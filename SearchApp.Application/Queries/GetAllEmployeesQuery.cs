using MediatR;
using SearchApp.Domain;

namespace SearchApp.Application
{
    public record GetAllEmployeesQuery() : IRequest<List<EmployeeDetailResponse>>;

    public class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetAllEmployeesQuery, List<EmployeeDetailResponse>>
    {
        public async Task<List<EmployeeDetailResponse>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetEmployees();
        }
    }
}
