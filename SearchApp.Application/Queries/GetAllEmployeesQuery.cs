using MediatR;
using SearchApp.Core.Entities;
using SearchApp.Core.Interface;

namespace SearchApp.Application
{
    public record GetAllEmployeesQuery() : IRequest<IEnumerable<EmployeeEntity>>;

    public class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeEntity>>
    {
        public async Task<IEnumerable<EmployeeEntity>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetEmployees();
        }
    }
}
