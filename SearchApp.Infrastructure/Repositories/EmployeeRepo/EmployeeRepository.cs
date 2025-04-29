using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SearchApp.Domain;
using SearchApp.Infrastructure.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SearchApp.Infrastructure.Repositories
{
    public class EmployeeRepository(AppDbContext dbContext) : IEmployeeRepository
    {
        public async Task<List<EmployeeDetailResponse>> GetEmployees()
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

            return new CommonResponse {UserId = entity.Id.ToString(), ResponseStatus = true, ResponseMessage = "Employee Added Successfully!" };
        }
        public async Task<List<EmployeeSearchResponse>> GetFilteredEmployeeData()
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
                PageNumber = SearchEmployeeRequest.PageNumber,
                PageSize = SearchEmployeeRequest.PageSize,
                SortBy = SearchEmployeeRequest.SortBy,
                SortOrder = SearchEmployeeRequest.SortOrder,
                ResultCount = ResultSetCount,
                SearchDate = DateTime.Now,
                UserId = SearchEmployeeRequest.UserId
            };
            dbContext.SearchHistory.Add(data);
            dbContext.SaveChanges();
        }
        public async Task<List<EmployeeSearchResponse>> EmpSearchByGetQueryAproach(EmpSearchByGetRequest request)
        {
            List<EmployeeEntity> EmployeeEntity = new();
            List<EmployeeSearchResponse> EmpList = new();
            if (!string.IsNullOrEmpty(request.FilterKey) && !string.IsNullOrEmpty(request.FilterValue))
            {
                if (request.PageSize <= 0 && request.PageNumber <= 0)
                {
                    request.PageNumber = 1;
                    request.PageSize = 10;
                    EmpList = EmpList.ToList();
                }

                if (request.FilterKey == "Name")
                {
                    if(!string.IsNullOrEmpty(request.SortBy) && !string.IsNullOrEmpty(request.SortOrder))
                    {
                        EmployeeEntity = dbContext.Employees.Where(x => x.Name.Contains(request.FilterValue) && x.IsActive == true)
                        .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
                    }                    
                }
                else if (request.FilterKey == "Department")
                {
                    EmployeeEntity = dbContext.Employees.Where(x => x.Department.Equals(request.FilterValue) && x.IsActive == true)
                        .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
                }
                else if (request.FilterKey == "Salary")
                {
                    EmployeeEntity = dbContext.Employees.Where(x => x.Salary.Equals(request.FilterValue) && x.IsActive == true)
                        .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
                }
                else if (request.FilterKey == "Date")
                {
                    DateTime FromDate = DateTime.Parse(request.FilterValue);
                    EmployeeEntity = dbContext.Employees.Where(x => x.CreatedDate.Value.Date == FromDate.Date && x.IsActive == true)
                        .Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
                }

                EmpList = EmployeeEntity.Select(x => new EmployeeSearchResponse
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
            }
            return EmpList;
        }
    }
}
