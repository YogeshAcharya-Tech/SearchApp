﻿using MediatR;
using SearchApp.Domain;

namespace SearchApp.Application
{
    public record AddEmployeeCommand(EmployeeEntity Employee) : IRequest<CommonResponse>;

    public class AddEmployeeCommandHandler(IEmployeeRepository employeeRepository) : IRequestHandler<AddEmployeeCommand, CommonResponse>
    {
        public async Task<CommonResponse> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            return await employeeRepository.AddEmployee(request.Employee);
        }
    }
}
