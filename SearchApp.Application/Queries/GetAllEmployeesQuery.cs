using MediatR;
using SearchApp.Core.Entities;
using SearchApp.Core.Interface;

namespace SearchApp.Application
{
    public record GetAllEmployeesQuery() : IRequest<IEnumerable<EmployeeDetailResponse>>;

    public class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDetailResponse>>
    {
        public async Task<IEnumerable<EmployeeDetailResponse>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetEmployees();
        }
    }
}
