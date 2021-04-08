using HR.Application.Services.Abstraction;
using HR.Data.DTO.EmployeeLeaves.Parameters;
using HR.Data.DTO.EmployeeLeaves.Results;
using HR.Data.DTO.Enums;
using HR.Data.Enums;
using HR.Data.Models;
using HR.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.Application.Services
{
    public class EmployeeLeavesService : IEmployeeLeavesService
    {
        private HrDbContext _dbContext;
        public EmployeeLeavesService(HrDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<EmployeeLeavesViewResult>> Get()
        {
            var employeeLeaves = await _dbContext.EmployeeLeaves
                .Include(e => e.Employee)
                .AsQueryable().ToListAsync();

            if (employeeLeaves.Count == 0)
                return null;

            List<EmployeeLeavesViewResult> employeeLeavesResult = new List<EmployeeLeavesViewResult>();
            foreach (var leave in employeeLeaves)
            {
                employeeLeavesResult.Add(new EmployeeLeavesViewResult
                {
                    EmployeeName = leave.Employee.Name,
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Duration = leave.Duration,
                    AttachmentFile = leave.AttachmentFile,
                    Status = leave.Status,
                    RejectReason = leave.RejectReason
                });
            }
            return employeeLeavesResult;
        }
        public async Task<List<EmployeeLeavesViewResult>> Get(EmployeeLeavesSearchParameters parametrs)
        {
            var query = _dbContext.EmployeeLeaves
                .Include(l => l.Employee)
                .AsQueryable();

            if (parametrs.EmployeeId.HasValue)
                query = query.Where(c => c.EmployeeId == parametrs.EmployeeId);

            if (parametrs.CategoryId.HasValue)
                query = query.Where(c => c.CategoryId == parametrs.CategoryId);

            if (parametrs.StartDate.HasValue)
                query = query.Where(c => c.StartDate.Equals(parametrs.StartDate));

            if (parametrs.EndDate.HasValue)
                query = query.Where(c => c.EndDate.Equals(parametrs.EndDate));

            if (parametrs.Status.HasValue)
                query = query.Where(c => c.Status.Equals(parametrs.Status));

            var employeeLeaves = await query.ToListAsync();

            if (employeeLeaves.Count == 0)
                return null;

            List<EmployeeLeavesViewResult> employeeLeaveViews = new List<EmployeeLeavesViewResult>();
            foreach (var leave in employeeLeaves)
            {
                employeeLeaveViews.Add(new EmployeeLeavesViewResult
                {
                    EmployeeName = leave.Employee?.Name,
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Duration = leave.Duration,
                    RejectReason = leave.RejectReason,
                    AttachmentFile = leave.AttachmentFile,
                    Status = leave.Status
                });
            }
            return employeeLeaveViews;
        }
        public async Task<EmployeeLeavesCreateResult> Create(EmployeeLeavesCreateParameters parameters)
        {
            var validationResult = await ValidateCreate(parameters);
            
            if (validationResult != EmployeeLeavesValidationStatus.Success)
            {
                return new EmployeeLeavesCreateResult
                {
                    ValidationStatus = validationResult,
                    CategoryId = parameters.CategoryId,
                    EmployeeId = parameters.EmployeeId,
                    StartDate = parameters.StartDate.Date,
                    EndDate = parameters.EndDate.Date,
                    AttachmentFile = parameters.AttachmentFile,
                    LeaveStatus = parameters.LeaveStatus,
                };
            }
            //var draft = GetDraft(parameters.EmployeeId);

            //if (draft != null)
            //{
            //    await Update(draft.i)
            //}
            else
            {
                var leaveType = await _dbContext.EmployeeLeaves.AsQueryable()
                    .Include(l => l.LeaveCategory)
                    .FirstOrDefaultAsync(d => d.CategoryId == parameters.CategoryId);

                var result = await _dbContext.EmployeeLeaves.AddAsync(new EmployeeLeave()
                {
                    StartDate = parameters.StartDate,
                    EndDate = parameters.EndDate,
                    Duration = parameters.EndDate.Subtract(parameters.StartDate).Days,
                    AttachmentFile = parameters.AttachmentFile,
                    Status = parameters.LeaveStatus,
                    CategoryId = parameters.CategoryId,
                    EmployeeId = parameters.EmployeeId,
                });
                await _dbContext.SaveChangesAsync();

                var leave = result.Entity;

                return new EmployeeLeavesCreateResult()
                {
                    CategoryId = parameters.CategoryId,
                    EmployeeId = parameters.EmployeeId,
                    StartDate = parameters.StartDate.Date,
                    EndDate = parameters.EndDate.Date,
                    AttachmentFile = parameters.AttachmentFile,
                    LeaveStatus = parameters.LeaveStatus,
                };
            }
        }
        public async Task<EmployeeLeavesUpdateResult> Update(int id, EmployeeLeavesUpdateParameters parameters)
        {
            var validationResult = await ValidateUpdate(id, parameters);

            var leave = await _dbContext.EmployeeLeaves
                      .Include(e => e.Employee)
                      .FirstOrDefaultAsync(c => c.Id == id);

            if (validationResult != EmployeeLeavesValidationStatus.Success)
            {
                return new EmployeeLeavesUpdateResult
                {
                    EmployeeName = leave.Employee?.Name,
                    ValidationStatus = validationResult,
                    StartDate = parameters.StartDate,
                    EndDate = parameters.EndDate,
                    AttachmentFile = parameters.AttachmentFile,
                    Status = LeaveStatus.Pending,
                    RejectReason = parameters.RejectReason,
                    Duration = parameters.EndDate.Day - parameters.StartDate.Day
            };
            }

            if (leave != null)
            {
                leave.StartDate = parameters.StartDate;
                leave.EndDate = parameters.EndDate;
                leave.AttachmentFile = parameters.AttachmentFile;
                leave.RejectReason = parameters.RejectReason;
                leave.Duration = parameters.EndDate.Day - parameters.StartDate.Day;
                leave.Status = parameters.LeaveStatus;

                await _dbContext.SaveChangesAsync();
            }
            return new EmployeeLeavesUpdateResult()
            {
                EmployeeName = leave.Employee?.Name,
                StartDate = parameters.StartDate,
                EndDate = parameters.EndDate,
                AttachmentFile = parameters.AttachmentFile,
                RejectReason = parameters.RejectReason,
                Status = parameters.LeaveStatus,
                Duration = parameters.EndDate.Day - parameters.StartDate.Day
        };
        }
        public async Task<int> Delete(int id)
        {
            var leave = await _dbContext.EmployeeLeaves.FirstOrDefaultAsync(e => e.Id == id);

            if (leave != null)
            {
                _dbContext.EmployeeLeaves.Remove(leave);
                await _dbContext.SaveChangesAsync();
                return id;
            }
            else
            {
                return 0;
            }
        }

        public async Task<EmployeeLeavesValidationStatus> ValidateCreate(EmployeeLeavesCreateParameters parameters)
        {

            if (await _dbContext.EmployeeLeaves.AnyAsync(l => l.EmployeeId == parameters.EmployeeId && l.Status == LeaveStatus.Draft))
                return EmployeeLeavesValidationStatus.DraftAlreadyExists;

            var hasDuplicate = await _dbContext.EmployeeLeaves.AnyAsync(l => l.CategoryId.Equals(parameters.CategoryId)
                                                    && l.EmployeeId.Equals(parameters.EmployeeId)
                                                    && l.StartDate == parameters.StartDate.Date
                                                    && l.EndDate == parameters.EndDate.Date
                                                    && (l.Status != LeaveStatus.Pending
                                                    || l.Status != LeaveStatus.Approved));

            var employeeDuration = _dbContext.EmployeeLeaves.Include(l => l.LeaveCategory)
                                                          .Where(e => e.EmployeeId.Equals(parameters.EmployeeId)
                                                           && e.LeaveCategory.Name.Equals(LeaveType.annual)
                                                           && e.Status == LeaveStatus.Approved)
                                                          .Sum(d => d.Duration);

            var duration = employeeDuration + (parameters.EndDate.Subtract(parameters.StartDate).Days);

            var leaveBalance = _dbContext.LeavesCategories
                                         .Where(l => l.Name.Equals(LeaveType.annual)).Select(l => l.Balance).FirstOrDefault();

            if (hasDuplicate)
            {
                return EmployeeLeavesValidationStatus.RequestAlreadyExists;
            }
            else if (employeeDuration.Equals(leaveBalance) || duration > leaveBalance)
            {
                return EmployeeLeavesValidationStatus.LeaveBalanceExceeded;
            }
            else
            {
                return EmployeeLeavesValidationStatus.Success;
            }
        }

        public async Task<EmployeeLeavesValidationStatus> ValidateUpdate(int id, EmployeeLeavesUpdateParameters parameters)
        {
            if (!await _dbContext.Employees
                .Include(e => e.Department)
                .AnyAsync(e => e.Department.ManagerId.HasValue && e.Department.ManagerId == parameters.ManagerId))
                return EmployeeLeavesValidationStatus.NotAuthorized;

            if(await _dbContext.EmployeeLeaves.AnyAsync(l => l.Id.Equals(id)
                                                     && l.Status == LeaveStatus.Approved
                                                     || l.Status == LeaveStatus.Pending))
            {
                return EmployeeLeavesValidationStatus.NotApplicable;
            }

            var employeeDuration = _dbContext.EmployeeLeaves.Include(l => l.LeaveCategory)
                                                            .Include(e => e.Employee)
                                                            .Where(e => (e.Id != id
                                                             && e.EmployeeId.Equals(parameters.EmployeeId)
                                                             && e.Status == LeaveStatus.Approved)
                                                             && e.LeaveCategory.Name.Equals(LeaveType.annual))
                                                            .Sum(d => d.Duration);

            var duration = employeeDuration + (parameters.EndDate.Subtract(parameters.StartDate).Days);

            var leaveBalance = _dbContext.LeavesCategories.Where(l => l.Name.Equals(LeaveType.annual)).Select(l => l.Balance).FirstOrDefault();

            if (employeeDuration.Equals(leaveBalance) || duration > leaveBalance)
            {
                return EmployeeLeavesValidationStatus.LeaveBalanceExceeded;
            }

            return EmployeeLeavesValidationStatus.Success;
        }

        public async Task<EmployeeLeavesCreateResult> GetDraft(int id)
        {
            var draft = await _dbContext.EmployeeLeaves.FirstOrDefaultAsync(l => l.EmployeeId.Equals(id)
                                                                                && (l.Status == LeaveStatus.Draft));
            if (draft != null)
            {
                return new EmployeeLeavesCreateResult()
                {
                    Id = draft.Id,
                    CategoryId = draft.CategoryId,
                    EmployeeId = draft.EmployeeId,
                    StartDate = draft.StartDate.Date,
                    EndDate = draft.EndDate.Date,
                    AttachmentFile = draft.AttachmentFile,
                    LeaveStatus = LeaveStatus.Draft,
                };
            }
            else
            {
                return null;
            }
        }
    }
}
