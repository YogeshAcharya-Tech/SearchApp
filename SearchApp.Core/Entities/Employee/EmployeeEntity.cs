using System.ComponentModel.DataAnnotations;

namespace SearchApp.Domain
{
    public class EmployeeEntity
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Employee name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Employee email is required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Employee mobile number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile number cannot be longer than 10 characters and less than 10 digits")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Employee Password is required")]
        public string Password { get; set; } = null!;
        public string Department { get; set; } = null!;
        public decimal? Salary { get; set; } = null!;
        public DateTime? CreatedDate { get; set; } = null!;
        public bool? IsActive { get; set; } = null!;
    }
}
