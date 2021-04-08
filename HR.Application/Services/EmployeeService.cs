using HR.Application.Services.Abstraction;
using HR.Data.DTO.Employees;
using HR.Data.DTO.Employees.Parameters;
using HR.Data.DTO.Employees.Results;
using HR.Data.DTO.Enums;
using HR.Data.Models;
using HR.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly HrDbContext _dbContext;
        private readonly IProtectionService _protectionService;
        public EmployeeService(HrDbContext dbContext, IProtectionService protectionService)
        {
            _dbContext = dbContext;
            _protectionService = protectionService;
        }
        public async Task<EmployeeViewResult> Get(int id)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return null;

            var department = await _dbContext.Departments.Include(d => d.Manager)
                            .FirstOrDefaultAsync(d => d.Id == employee.DepartmentId);

            return new EmployeeViewResult
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                JobTitle = employee.JobTitle,
                DepartmentName = department?.Name,
                ManagerName = department.Manager?.Name
            };
        }
        public async Task<List<EmployeeViewResult>> Get(EmployeeSearchParametrs parametrs)
        {
            var query = _dbContext.Employees
                .Include(d => d.Department)
                .Include(d => d.Department.Manager)
                .AsQueryable();
            
            if (parametrs.Id.HasValue)
                query = query.Where(e => e.Id == parametrs.Id);

            if (!string.IsNullOrWhiteSpace(parametrs.Name))
                query = query.Where(e => e.Name.ToLower().Contains(parametrs.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(parametrs.Email))
                query = query.Where(e => e.Email.Contains(parametrs.Email));

            if (!string.IsNullOrWhiteSpace(parametrs.JobTitle))
                query = query.Where(e => e.JobTitle.ToLower().Equals(parametrs.JobTitle.ToLower()));

            if (parametrs.DepartmentId.HasValue)
                query = query.Where(e => e.DepartmentId.HasValue && e.DepartmentId == parametrs.DepartmentId);
     
            if (parametrs.ManagerId.HasValue)
                query = query.Where(e => e.Department.ManagerId.HasValue && e.Department.ManagerId == parametrs.ManagerId);


            var employees = await query.ToListAsync();

            if (employees.Count == 0)
                return null;

            List<EmployeeViewResult> employeeViews = new List<EmployeeViewResult>();
            foreach (var employee in employees)
            {
                employeeViews.Add(new EmployeeViewResult
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    PhoneNumber = employee.PhoneNumber,
                    Email = employee.Email,
                    JobTitle = employee.JobTitle,
                    DepartmentName = employee.Department?.Name,
                    ManagerName = employee.Department?.Manager?.Name
                });
            }
            return employeeViews;
        }
        public async Task<EmployeeUpdateResult> Update(int id, EmployeeCreateParameters parameters)
        {
            var validationResult = await Validate(id, parameters);

            if (validationResult != EmployeeValidationStatus.Success)
            {
                return new EmployeeUpdateResult
                {
                    Status = validationResult
                };
            }
            var employee = await _dbContext.Employees
                .Include(d => d.Department)
                .Include(d => d.Department.Manager)
                .FirstOrDefaultAsync(e => e.Id == id);
            var department = await _dbContext.Departments.Include(d => d.Manager).FirstOrDefaultAsync(d => d.Id == parameters.DepartmentId);
            
            if (department == null)
            {
                employee.Name = parameters.Name;
                employee.PhoneNumber = parameters.PhoneNumber;
                employee.Email = parameters.Email;
                employee.Password = parameters.Password;
                employee.JobTitle = parameters.JobTitle;

                await _dbContext.SaveChangesAsync();

                return new EmployeeUpdateResult
                {
                    Status = EmployeeValidationStatus.Success,
                    Data = new EmployeeViewResult
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        PhoneNumber = employee.PhoneNumber,
                        Email = employee.Email,
                        JobTitle = employee.JobTitle,
                        DepartmentName = employee.Department?.Name,
                        ManagerName = employee.Department?.Manager?.Name
                    }
                };
            }
            else
            {
                employee.Name = parameters.Name;
                employee.PhoneNumber = parameters.PhoneNumber;
                employee.Email = parameters.Email;
                employee.Password = parameters.Password;
                employee.JobTitle = parameters.JobTitle;
                employee.Department = department;

                await _dbContext.SaveChangesAsync();

                return new EmployeeUpdateResult
                {
                    Status = EmployeeValidationStatus.Success,
                    Data = new EmployeeViewResult
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        PhoneNumber = employee.PhoneNumber,
                        Email = employee.Email,
                        JobTitle = employee.JobTitle,
                        DepartmentName = department.Name,
                        ManagerName = department.Manager?.Name
                    }
                };
            }
        }
        public async Task<int> Delete(int id)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
                return id;
            }
            else
            {
                return 0;
            }
        }
        private async Task<EmployeeValidationStatus> Validate(int id, EmployeeCreateParameters parameters)
        {
            var findEmail = await _dbContext.Employees.AnyAsync(e => (id == 0 || e.Id != id) && e.Email.ToLower().Equals(parameters.Email.ToLower()));

            if (findEmail)
            {
                return EmployeeValidationStatus.EmailAlreadyExists;
            }
            else
            {
                return EmployeeValidationStatus.Success;
            }
        }
        public async Task<EmployeeCreateResult> Register(EmployeeCreateParameters parameters)
        {
            parameters.Password = _protectionService.ComputeHash(parameters.Password);
            return await Create(parameters);
        }
        public async Task<EmployeeViewResult> login(string email, string password)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);

            if (employee == null)
                return null;
            else
            {
                if (_protectionService.VerifyHashedPassword(password, employee.Password))
                {
                    return new EmployeeViewResult
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        PhoneNumber = employee.PhoneNumber,
                        Email = employee.Email,
                        JobTitle = employee.JobTitle
                    };
                }
                else return null;
            }
        }


        private async Task<EmployeeCreateResult> Create(EmployeeCreateParameters parameters)
        {
            var validationResult = await Validate(0, parameters);
            var department = await _dbContext.Departments.Include(e => e.Manager)
                .FirstOrDefaultAsync(d => d.Id == parameters.DepartmentId);

            if (validationResult != EmployeeValidationStatus.Success)
            {
                return new EmployeeCreateResult
                {
                    Status = validationResult
                };
            }
            var result = await _dbContext.Employees.AddAsync(new Employee()
            {
                Name = parameters.Name,
                PhoneNumber = parameters.PhoneNumber,
                Email = parameters.Email,
                Password = parameters.Password,
                JobTitle = parameters.JobTitle,
                Department = department,
            });
            await _dbContext.SaveChangesAsync();

            var employee = result.Entity;
            if (department == null)
            {
                return new EmployeeCreateResult(EmployeeValidationStatus.Success)
                {
                    Data = new EmployeeViewResult
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        PhoneNumber = parameters.PhoneNumber,
                        Email = parameters.Email,
                        JobTitle = parameters.JobTitle,
                    }
                };
            }
            else
            {
                return new EmployeeCreateResult(EmployeeValidationStatus.Success)
                {
                    Data = new EmployeeViewResult
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        PhoneNumber = parameters.PhoneNumber,
                        Email = parameters.Email,
                        JobTitle = parameters.JobTitle,
                        DepartmentName = department?.Name,
                        ManagerName = department.Manager?.Name
                    }
                };
            }

        }
    }
}