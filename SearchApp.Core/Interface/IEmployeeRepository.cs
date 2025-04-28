using SearchApp.Core;

namespace SearchApp.Core
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDetailResponse>> GetEmployees();
        Task<EmployeeDetailResponse> GetEmployeeById(string UserId);
        Task<CommonResponse> AddEmployee(EmployeeEntity entity);
        Task<IEnumerable<EmployeeSearchResponse>> GetFilteredEmployeeData();
        void SaveSearchHistory(SearchEmployeeRequest SearchEmployeeRequest, int ResultSetCount);
    }
}
