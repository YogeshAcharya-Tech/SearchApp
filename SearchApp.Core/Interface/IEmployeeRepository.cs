namespace SearchApp.Domain
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeDetailResponse>> GetEmployees();
        Task<EmployeeDetailResponse> GetEmployeeById(string UserId);
        Task<CommonResponse> AddEmployee(EmployeeEntity entity);
        Task<List<EmployeeSearchResponse>> GetFilteredEmployeeData();
        void SaveSearchHistory(SearchEmployeeRequest SearchEmployeeRequest, int ResultSetCount);
        Task<List<EmployeeSearchResponse>> EmpSearchByGetQueryAproach(EmpSearchByGetRequest request);
    }
}
