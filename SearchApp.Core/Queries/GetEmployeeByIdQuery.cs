using MediatR;

namespace SearchApp.Core
{
    public record GetEmployeeByIdQuery(string UserId) : IRequest<EmployeeDetailResponse>;

    public class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetEmployeeByIdQuery, EmployeeDetailResponse>
    {
        public async Task<EmployeeDetailResponse> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetEmployeeById(request.UserId);
        }
    }
}
