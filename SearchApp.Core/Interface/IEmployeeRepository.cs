using SearchApp.Core.Entities;

namespace SearchApp.Core.Interface
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeEntity>> GetEmployees();
        Task<EmployeeEntity> GetEmployeeById(Guid Id);
        Task<CommonResponse> AddEmployee(EmployeeEntity entity);
        Task<IEnumerable<EmployeeSearchResponse>> GetFilteredEmployeeData();
        void SaveSearchHistory(SearchEmployeeRequest SearchEmployeeRequest, int ResultSetCount);
    }
}
