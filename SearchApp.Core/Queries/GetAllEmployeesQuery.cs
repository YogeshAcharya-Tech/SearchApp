using MediatR;

namespace SearchApp.Core
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
