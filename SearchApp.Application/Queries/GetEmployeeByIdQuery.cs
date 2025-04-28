using MediatR;
using SearchApp.Core.Entities;
using SearchApp.Core.Interface;

namespace SearchApp.Application.Queries
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
