using HR.Application.Services.Abstraction;
using HR.Data.DTO.Departments.Parameters;
using HR.Data.DTO.Departments.Results;
using HR.Data.DTO.Employees.Results;
using HR.Data.DTO.Enums;
using HR.Data.Models;
using HR.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private HrDbContext _dbContext;
        public DepartmentService(HrDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //public async Task<DepartmentViewResult> Get(int id)
        //{

        //    var department = await _dbContext.Departments.FirstOrDefaultAsync(d => d.Id == id);

        //    if (department == null)
        //        return null;
        //    var managerId = department.ManagerId;
        //    var manager = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == managerId);
        //    if (manager != null)
        //    {
        //        return new DepartmentViewResult
        //        {
        //            Id = department.Id,
        //            Name = department.Name,
        //            ManagerName = manager.Name
        //        };
        //    }
        //    else
        //    {
        //        return new DepartmentViewResult
        //        {
        //            Id = department.Id,
        //            Name = department.Name,
        //        };
        //    }
        //}


        //auto Mapper
        public async Task<DepartmentViewResult> Get(DepartmentSearchParameters parameters)
        {
            var query = _dbContext.Departments
                .Include(d => d.Manager)
                .Include(d => d.Employees)
                .AsQueryable();

            List<EmployeeViewResult> employees = new List<EmployeeViewResult>();
            if (parameters.Id.HasValue)
            {
                query = query.Where(d => d.Id== parameters.Id);
            }
            if (parameters.ManagerId.HasValue)
            {
                query = query.Where(e => e.ManagerId.Equals(parameters.ManagerId));
            }
            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                query = query.Where(e => e.Name.Contains(parameters.Name));
            }

            var department = await query.FirstOrDefaultAsync();


            if (department == null)
                return null;

            if(department.Employees != null && department.Employees.Any())
            {
                foreach (var item in department.Employees)
                {

                    employees.Add(new EmployeeViewResult
                    {
                        DepartmentName = department.Name,
                        Id = item.Id,
                        Name = item.Name,
                        Email = item.Email,
                        JobTitle = item.JobTitle,
                        PhoneNumber = item.PhoneNumber,
                        ManagerName = department.Manager?.Name,
                    });
                }
            }

            return new DepartmentViewResult
            {
                Id = department.Id,
                Name = department.Name,
                ManagerName = department.Manager?.Name,
                Employees = employees
            };

        }
        public async Task<DepartmentCreateResult> Create(DepartmentCreateParameters parameters)
        {
            var validationResult = await Validate(0, parameters);
            var manager = await _dbContext.Employees.FindAsync(parameters.ManagerId);
            if (validationResult != DepartmentValidationStatus.Success)
            {
                return new DepartmentCreateResult
                {
                    Status = validationResult
                };
            }
            var result = await _dbContext.Departments.AddAsync(new Department()
            {
                Employees = new List<Employee>(),
                Manager = manager,
                Name = parameters.Name,
            });
            await _dbContext.SaveChangesAsync();

            var department = result.Entity;
            if (manager == null)
            {
                return new DepartmentCreateResult
                {
                    Data = new DepartmentViewResult
                    {
                        Id = department.Id,
                        Name = department.Name,
                    }
                };
            }
            else
            {
                return new DepartmentCreateResult
                {
                    Data = new DepartmentViewResult
                    {
                        Id = department.Id,
                        Name = department.Name,
                        ManagerName = manager.Name
                    }
                };
            }
        }
        public async Task<DepartmentUpdateResult> Update(int id, DepartmentCreateParameters parameters)
        {
            var validationResult = await Validate(id, parameters);
            
            if (validationResult != DepartmentValidationStatus.Success)
                return new DepartmentUpdateResult(validationResult);


            var department = await _dbContext.Departments
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(d => d.Id == id);
            

            if (department == null)
                return null;


            if (!string.IsNullOrWhiteSpace(parameters.Name))
                department.Name = parameters.Name;

            if (parameters.ManagerId.HasValue)
            {
                var manager = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == parameters.ManagerId.Value);
                if(manager != null)
                {
                    department.Manager = manager;
                    department.ManagerId = department.ManagerId;
                }
            }

            await _dbContext.SaveChangesAsync();
            return new DepartmentUpdateResult(DepartmentValidationStatus.Success)
            {
                Id = department.Id,
                Name = department.Name,
                ManagerName = department.Manager?.Name
            };
        }
        public async Task<int> Delete(int id)
        {
            var department = await _dbContext.Departments.FirstOrDefaultAsync(e => e.Id == id);

            // Must be validation method, here just do remove.
            if (department == null || await _dbContext.Employees.AnyAsync(e => e.DepartmentId == id))
                return 0;


            _dbContext.Departments.Remove(department);
            await _dbContext.SaveChangesAsync();
            return id;
        }
        private async Task<DepartmentValidationStatus> Validate(int id, DepartmentCreateParameters parameters)
        {

            var findName = await _dbContext.Departments.AnyAsync(d => (id == 0 || d.Id != id) && d.Name.ToLower().Equals(parameters.Name.ToLower()));
            var findManager = await _dbContext.Departments.FirstOrDefaultAsync(d => d.ManagerId.Equals(parameters.ManagerId));

            if (findName)
            {
                return DepartmentValidationStatus.DepartmentNameAlreadyExists;

            }
            else if (findManager != null && findManager.ManagerId == parameters.ManagerId)
            {
                return DepartmentValidationStatus.DepartmentManagerAlreadyExists;
            }
            else
            {
                return DepartmentValidationStatus.Success;
            }
        }
    }
}
