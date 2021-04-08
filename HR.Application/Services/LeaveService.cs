using HR.Application.Services.Abstraction;
using HR.Data.DTO.Enums;
using HR.Data.DTO.Leaves.Parameters;
using HR.Data.DTO.Leaves.Results;
using HR.Data.Models;
using HR.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Services
{
    public class LeaveService : ILeaveService
    {
        private HrDbContext _dbContext;
        public LeaveService(HrDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<LeaveViewResult>> Get()
        {
            var leaves = await _dbContext.LeavesCategories.ToListAsync();

            if (leaves.Count == 0)
                return null;

            List<LeaveViewResult> leaveViewResults = new List<LeaveViewResult>();
            foreach (var leave in leaves)
            {
                leaveViewResults.Add(new LeaveViewResult
                {
                    Id = leave.Id,
                    Name = leave.Name,
                    MaxDuration = leave.MaxDuration,
                    Balance = leave.Balance
                }); 
            }
            return leaveViewResults;
        }
        public async Task<List<LeaveViewResult>> Get(LeaveSearchParameters parametrs)
        {
            var query = _dbContext.LeavesCategories.AsQueryable();

            if (parametrs.Id.HasValue)
                query = query.Where(c => c.Id == parametrs.Id);


            if (parametrs.Name != 0)
                query = query.Where(c => c.Name.Equals(parametrs.Name));


            var leaves = await query.ToListAsync();

            if (leaves.Count == 0)
                return null;

            List<LeaveViewResult> leaveViews = new List<LeaveViewResult>();
            foreach (var leave in leaves)
            {
                leaveViews.Add(new LeaveViewResult
                {
                    Id = leave.Id,
                    Name = leave.Name,
                    MaxDuration = leave.MaxDuration,
                    Balance = leave.Balance
                });
            }
            return leaveViews;
        }
        public async Task<LeaveCreateResult> Create(LeaveCreateParameters parameters)
        {
            var validationResult = await Validate(parameters);

            if (validationResult != LeaveValidationStatus.Success)
            {
                return new LeaveCreateResult
                {
                    Status = validationResult
                };
            }
            var result = await _dbContext.LeavesCategories.AddAsync(new LeaveCategory()
            {
                Name = parameters.Name,
                MaxDuration = parameters.MaxDuration,
                Balance = parameters.Balance

            });
            await _dbContext.SaveChangesAsync();

            var leaves = result.Entity;
            return new LeaveCreateResult(LeaveValidationStatus.Success)
            {
                Data = new LeaveViewResult
                {
                    Id = leaves.Id,
                    Name = leaves.Name,
                    MaxDuration = leaves.MaxDuration,
                    Balance = leaves.Balance
                }
            };
        }
        public async Task<LeaveUpdateResult> Update(int id, LeaveUpdateParameters parameters)
        {
            var leave = await _dbContext.LeavesCategories.FirstOrDefaultAsync(c => c.Id == id);

            if (leave != null)
            {
                leave.MaxDuration = parameters.MaxDuration;
                leave.Balance = parameters.Balance;

                await _dbContext.SaveChangesAsync();
            }
            return new LeaveUpdateResult()
            {
                Data = new LeaveViewResult
                {
                    Id = leave.Id,
                    Name = leave.Name,
                    MaxDuration = leave.MaxDuration,
                    Balance = leave.Balance
                }
            };
        }
        public async Task<int> Delete(int id)
        {
            var leave = await _dbContext.LeavesCategories.FirstOrDefaultAsync(e => e.Id == id);

            if (leave != null)
            {
                _dbContext.LeavesCategories.Remove(leave);
                await _dbContext.SaveChangesAsync();
                return id;
            }
            else
            {
                return 0;
            }
        }
        private async Task<LeaveValidationStatus> Validate(LeaveCreateParameters parameters)
        {
            var findLeave = await _dbContext.LeavesCategories.AnyAsync(e => e.Name.Equals(parameters.Name));

            if (findLeave)
            {
                return LeaveValidationStatus.LeaveNameAlreadyExists;
            }
            else
            {
                return LeaveValidationStatus.Success;
            }
        }
    }
}
