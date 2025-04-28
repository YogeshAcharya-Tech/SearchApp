using Microsoft.EntityFrameworkCore;
using SearchApp.Core.Entities;
using SearchApp.Core.Interface;
using SearchApp.Infrastructure.Data;

namespace SearchApp.Infrastructure.Repositories
{
    public class EmployeeRepository(AppDbContext dbContext) : IEmployeeRepository
    {
        public async Task<IEnumerable<EmployeeDetailResponse>> GetEmployees()
        {
            var data = await dbContext.Employees.Where(x => x.IsActive == true).ToListAsync();

            List<EmployeeDetailResponse> EmpList = data.Select(x => new EmployeeDetailResponse
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Department = x.Department,
                Salary = x.Salary,
                CreatedDate = x.CreatedDate,
                IsActive = x.IsActive
            }).ToList();

            return EmpList;
        }
        public async Task<EmployeeDetailResponse> GetEmployeeById(string UserId)
        {
            var data = await dbContext.Employees.FirstOrDefaultAsync(x=> x.Id.ToString() == UserId && x.IsActive == true);
            if (data != null)
            {
                return new EmployeeDetailResponse
                {
                    Id = data.Id.ToString(),
                    Name = data.Name,
                    Email = data.Email,
                    Phone = data.Phone,
                    Department = data.Department,
                    Salary = data.Salary,
                    CreatedDate = data.CreatedDate,
                    IsActive = data.IsActive
                };
            }
            else
            {
                return null;
            }
        }
        public async Task<CommonResponse> AddEmployee(EmployeeEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.IsActive = true;
            entity.CreatedDate = DateTime.Now;
            dbContext.Employees.Add(entity);
            dbContext.SaveChanges();

            return new CommonResponse { ResponseStatus = true, ResponseMessage = "Employee Added Successfully!" };
        }
        public async Task<IEnumerable<EmployeeSearchResponse>> GetFilteredEmployeeData()
        {
            var data = dbContext.Employees.Where(x => x.IsActive == true).ToList();
            List<EmployeeSearchResponse> EmpList = data.Select(x=> new EmployeeSearchResponse 
            { 
                Id = x.Id.ToString(),
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Department = x.Department,
                Salary = x.Salary,
                CreatedDate = x.CreatedDate,
                IsActive = x.IsActive
            }).ToList();
            return EmpList;
        }
        public async void SaveSearchHistory(SearchEmployeeRequest SearchEmployeeRequest, int ResultSetCount)
        {
            var data = new SearchHistory
            {
                Department = SearchEmployeeRequest.Department,
                Salary = SearchEmployeeRequest.Salary,
                Name = SearchEmployeeRequest.Name,
                FromDate = SearchEmployeeRequest.FromDate,
                ToDate = SearchEmployeeRequest.ToDate,
                RecordsPerRequest = SearchEmployeeRequest.RecordsPerRequest,
                SortBy = SearchEmployeeRequest.SortBy,
                SortOrder = SearchEmployeeRequest.SortOrder,
                ResultCount = ResultSetCount,
                SearchDate = DateTime.Now,
                UserId = SearchEmployeeRequest.UserId
            };
            dbContext.SearchHistory.Add(data);
            dbContext.SaveChanges();
        }
    }
}
