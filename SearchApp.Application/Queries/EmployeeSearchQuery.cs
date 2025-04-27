using MediatR;
using SearchApp.Core.Entities;
using SearchApp.Core.Interface;

namespace SearchApp.Application
{
    public record EmployeeSearchQuery(SearchEmployeeRequest SearchEmployeeRequest) : IRequest<IEnumerable<EmployeeSearchResponse>>;

    public class EmployeeSearchHandler(IEmployeeRepository _employeeRepository) : IRequestHandler<EmployeeSearchQuery, IEnumerable<EmployeeSearchResponse>>
    {
        public async Task<IEnumerable<EmployeeSearchResponse>> Handle(EmployeeSearchQuery request, CancellationToken cancellationToken)
        {
            List<EmployeeSearchResponse> EmpList = new List<EmployeeSearchResponse>();

            // Get all employees
            var AllEmployees = await _employeeRepository.GetFilteredEmployeeData();


            if (request.SearchEmployeeRequest.FromDate != null && request.SearchEmployeeRequest.ToDate != null)
            {
                EmpList = AllEmployees.Where(p => p.CreatedDate.Value.Date >= request.SearchEmployeeRequest.FromDate.Value.Date && p.CreatedDate.Value.Date <= request.SearchEmployeeRequest.ToDate.Value.Date).ToList();
            }

            // Apply filter based input value
            if (!string.IsNullOrEmpty(request.SearchEmployeeRequest.Name))
            {
                if(EmpList.Count <= 0)
                {
                    EmpList = AllEmployees.Where(p => p.Name.Contains(request.SearchEmployeeRequest.Name))?.ToList();
                }
                else
                {
                    EmpList = EmpList.Where(p => p.Name.Contains(request.SearchEmployeeRequest.Name))?.ToList();
                }
            }
            if (request.SearchEmployeeRequest.Salary != null && request.SearchEmployeeRequest.Salary > 0)
            {
                if(EmpList.Count <= 0)
                {
                    EmpList = AllEmployees.Where(p => p.Salary.Equals(request.SearchEmployeeRequest.Salary))?.ToList();
                }
                else
                {
                    EmpList = EmpList.Where(p => p.Salary.Equals(request.SearchEmployeeRequest.Salary))?.ToList();
                }                
            }            
            if (!string.IsNullOrEmpty(request.SearchEmployeeRequest.Department))
            {
                if(EmpList.Count <= 0)
                {
                    EmpList = AllEmployees.Where(p => p.Department.Equals(request.SearchEmployeeRequest.Department))?.ToList();
                }
                else
                {
                    EmpList = EmpList.Where(p => p.Department.Equals(request.SearchEmployeeRequest.Department))?.ToList();
                }                
            }

            return EmpList;
        }
    }
}
