using Microsoft.EntityFrameworkCore;
using SearchApp.Domain;
using SearchApp.Infrastructure.Data;

namespace SearchApp.Infrastructure.Repositories
{
    public class AuthRepository(AppDbContext dbContext) : IAuthRepository
    {
        public async Task<CommonResponse> GetAccessToken(AuthRequest AuthRequest)
        {
            /// <summary> Checking if employee table is empty then inserting Admin credetials for 1st time only if username and password as per mentioned in API document, We can remove this code after atleast one record inserted in Db
            /// </summary>
            var IsExistEmpData = await CheckIsEmpDataExists(AuthRequest);

            if(IsExistEmpData.ResponseStatus)
            {
                var data = await dbContext.Employees.FirstOrDefaultAsync(x => x.Email == AuthRequest.Username && x.Password == AuthRequest.Password);

                if (data != null)
                {
                    return new CommonResponse { UserId = data.Id.ToString(), ResponseStatus = true, ResponseMessage = "Logged in successfully!" };
                }
                else
                {
                    return new CommonResponse { ResponseStatus = false, ResponseMessage = "Failed: Invalid username or password" };
                }
            }
            else
            {
                return new CommonResponse { ResponseStatus = false, ResponseMessage = "Failed: Invalid username or password" };
            }
        }
        public async Task<CommonResponse> CheckIsEmpDataExists(AuthRequest AuthRequest)
        {
            CommonResponse CommonResponse = new ();

            int Count = dbContext.Employees.ToList().Count();

            if(Count <= 0)
            {
                if (AuthRequest.Username.Equals("Admin@Yahoo.com") && AuthRequest.Password.Equals("Admin@786"))
                {
                    EmployeeEntity entity = new EmployeeEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin",
                        Email = AuthRequest.Username,
                        Phone = "8767543423",
                        Password = AuthRequest.Password,
                        Department = "Admin",
                        Salary = Convert.ToDecimal("48000"),
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                    };
                    dbContext.Employees.Add(entity);
                    dbContext.SaveChanges();

                    CommonResponse = new CommonResponse
                    {
                        ResponseStatus = true,
                        ResponseMessage = "Employee Added Successfully!"
                    };
                }
                else
                {
                    CommonResponse = new CommonResponse
                    {
                        ResponseStatus = false,
                        ResponseMessage = "Invalid Username or Password"
                    };
                }                
            }
            else
            {
                CommonResponse = new CommonResponse
                {
                    ResponseStatus = true,
                    ResponseMessage = "Employee data is exists!"
                };
            }
            return CommonResponse;
        }
    }
}
