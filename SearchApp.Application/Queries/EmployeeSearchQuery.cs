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
            var SearchEmployeeRequest = request.SearchEmployeeRequest;
            List<EmployeeSearchResponse> EmpList = new List<EmployeeSearchResponse>();
            
            // Get all employees
            var AllEmployees = await _employeeRepository.GetFilteredEmployeeData();

            if (!string.IsNullOrEmpty(SearchEmployeeRequest.FromDate) && !string.IsNullOrEmpty(SearchEmployeeRequest.ToDate))
            {
                DateTime FromDate = DateTime.Parse(SearchEmployeeRequest.FromDate);
                DateTime ToDate = DateTime.Parse(SearchEmployeeRequest.ToDate);
                EmpList = AllEmployees.Where(x => x.CreatedDate.Value.Date >= FromDate.Date && x.CreatedDate.Value.Date <= ToDate.Date).ToList();
            }

            // Apply filter based input value
            if (!string.IsNullOrEmpty(SearchEmployeeRequest.Name))
            {
                if(EmpList.Count <= 0)
                {
                    EmpList = AllEmployees.Where(p => p.Name.Contains(SearchEmployeeRequest.Name))?.ToList();
                }
                else
                {
                    EmpList = EmpList.Where(p => p.Name.Contains(SearchEmployeeRequest.Name))?.ToList();
                }
            }
            if (SearchEmployeeRequest.Salary != null && SearchEmployeeRequest.Salary > 0)
            {
                if(EmpList.Count <= 0)
                {
                    EmpList = AllEmployees.Where(p => p.Salary.Equals(SearchEmployeeRequest.Salary))?.ToList();
                }
                else
                {
                    EmpList = EmpList.Where(p => p.Salary.Equals(SearchEmployeeRequest.Salary))?.ToList();
                }                
            }            
            if (!string.IsNullOrEmpty(SearchEmployeeRequest.Department))
            {
                if(EmpList.Count <= 0)
                {
                    EmpList = AllEmployees.Where(p => p.Department.Equals(SearchEmployeeRequest.Department))?.ToList();
                }
                else
                {
                    EmpList = EmpList.Where(p => p.Department.Equals(SearchEmployeeRequest.Department))?.ToList();
                }                
            }

            if(EmpList.Count > 0 && SearchEmployeeRequest.RecordsPerRequest > 0)
            {
                EmpList = EmpList.Take(SearchEmployeeRequest.RecordsPerRequest).ToList();                
            }

            var FinalList = SortEmpList(EmpList, SearchEmployeeRequest);

            // Save employees search history
            _employeeRepository.SaveSearchHistory(SearchEmployeeRequest, FinalList.Count);

            return FinalList;
        }
        private List<EmployeeSearchResponse> SortEmpList(List<EmployeeSearchResponse> EmpList, SearchEmployeeRequest SearchEmployeeRequest)
        {
            if (!string.IsNullOrEmpty(SearchEmployeeRequest.SortBy) && !string.IsNullOrEmpty(SearchEmployeeRequest.SortOrder))
            {
                if (SearchEmployeeRequest.SortBy.ToLower() == "department")
                {
                    if (SearchEmployeeRequest.SortOrder.ToLower() == "desc")
                    {
                        EmpList = EmpList.OrderByDescending(x => x.Department).ToList();
                    }
                    else
                    {
                        EmpList = EmpList.OrderBy(x => x.Department).ToList();
                    }
                }
                if (SearchEmployeeRequest.SortBy.ToLower() == "date")
                {
                    if (SearchEmployeeRequest.SortOrder.ToLower() == "desc")
                    {
                        EmpList = EmpList.OrderByDescending(x => x.CreatedDate).ToList();
                    }
                    else
                    {
                        EmpList = EmpList.OrderBy(x => x.CreatedDate).ToList();
                    }
                }
                if (SearchEmployeeRequest.SortBy.ToLower() == "salary")
                {
                    if (SearchEmployeeRequest.SortOrder.ToLower() == "desc")
                    {
                        EmpList = EmpList.OrderByDescending(x => x.Salary).ToList();
                    }
                    else
                    {
                        EmpList = EmpList.OrderBy(x => x.Salary).ToList();
                    }
                }
                if (SearchEmployeeRequest.SortBy.ToLower() == "name")
                {
                    if (SearchEmployeeRequest.SortOrder.ToLower() == "desc")
                    {
                        EmpList = EmpList.OrderByDescending(x => x.Name).ToList();
                    }
                    else
                    {
                        EmpList = EmpList.OrderBy(x => x.Name).ToList();
                    }
                }
            }
            return EmpList;
        }
    }
}
