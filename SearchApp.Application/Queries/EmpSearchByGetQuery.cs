using MediatR;
using SearchApp.Domain;

namespace SearchApp.Application
{
    public class EmpSearchByGetQuery : IRequest<List<EmployeeSearchResponse>>
    {
        public EmpSearchByGetRequest Obj { get; } = new();

        public EmpSearchByGetQuery(string filterKey, string filterValue, string sortBy, string sortOrder, int pageNumber, int pageSize, string UserId)
        {
            Obj.FilterKey = filterKey;
            Obj.FilterValue = filterValue;
            Obj.SortBy = sortBy;
            Obj.SortOrder = sortOrder;
            Obj.PageNumber = pageNumber;
            Obj.PageSize = pageSize;
            Obj.UserId = UserId;
        }
    }

    public class EmpSearchByGetQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<EmpSearchByGetQuery, List<EmployeeSearchResponse>>
    {
        public async Task<List<EmployeeSearchResponse>> Handle(EmpSearchByGetQuery request, CancellationToken cancellationToken)
        {
            var EmpList = await employeeRepository.EmpSearchByGetQueryAproach(request.Obj);

            var FinalList = SortEmpList(EmpList, request.Obj);

            return FinalList;
        }
        private List<EmployeeSearchResponse> SortEmpList(List<EmployeeSearchResponse> EmpList, EmpSearchByGetRequest Request)
        {
            if (!string.IsNullOrEmpty(Request.SortBy) && !string.IsNullOrEmpty(Request.SortOrder))
            {
                if (Request.SortBy.ToLower() == "department")
                {
                    if (Request.SortOrder.ToLower() == "desc")
                    {
                        EmpList = EmpList.OrderByDescending(x => x.Department).ToList();
                    }
                    else
                    {
                        EmpList = EmpList.OrderBy(x => x.Department).ToList();
                    }
                }
                if (Request.SortBy.ToLower() == "date")
                {
                    if (Request.SortOrder.ToLower() == "desc")
                    {
                        EmpList = EmpList.OrderByDescending(x => x.CreatedDate).ToList();
                    }
                    else
                    {
                        EmpList = EmpList.OrderBy(x => x.CreatedDate).ToList();
                    }
                }
                if (Request.SortBy.ToLower() == "salary")
                {
                    if (Request.SortOrder.ToLower() == "desc")
                    {
                        EmpList = EmpList.OrderByDescending(x => x.Salary).ToList();
                    }
                    else
                    {
                        EmpList = EmpList.OrderBy(x => x.Salary).ToList();
                    }
                }
                if (Request.SortBy.ToLower() == "name")
                {
                    if (Request.SortOrder.ToLower() == "desc")
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
