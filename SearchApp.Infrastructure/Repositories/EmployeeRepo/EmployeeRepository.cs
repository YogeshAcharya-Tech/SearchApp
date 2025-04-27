using Microsoft.EntityFrameworkCore;
using SearchApp.Core.Entities;
using SearchApp.Core.Interface;
using SearchApp.Infrastructure.Data;

namespace SearchApp.Infrastructure.Repositories
{
    public class EmployeeRepository(AppDbContext dbContext) : IEmployeeRepository
    {
        public async Task<IEnumerable<EmployeeEntity>> GetEmployees()
        {
            return await dbContext.Employees.ToListAsync();
        }

        public async Task<EmployeeEntity> GetEmployeeById(Guid Id)
        {
            var data = await dbContext.Employees.FirstOrDefaultAsync(x=> x.Id == Id);
            return data;
        }

        public async Task<CommonResponse> AddEmployee(EmployeeEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.IsActive = true;
            dbContext.Employees.Add(entity);
            dbContext.SaveChanges();

            return new CommonResponse { ResponseStatus = true, ResponseMessage = "Employee Added Successfully!" };
        }
        public async Task<IEnumerable<EmployeeSearchResponse>> GetFilteredEmployeeData()
        {
            var data = dbContext.Employees.ToList();
            List<EmployeeSearchResponse> EmpList = data.Select(x=> new EmployeeSearchResponse { Id = x.Id.ToString(),Name = x.Name, Email = x.Email, Phone = x.Phone, Department = x.Department, Salary = x.Salary, CreatedDate = x.CreatedDate }).ToList();
            return EmpList;
        }
    }
}
