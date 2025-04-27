using System.ComponentModel.DataAnnotations;

namespace SearchApp.Core.Entities
{
    public class SearchEmployeeRequest
    {
        public string? Department { get; set; }
        public string? Name { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public int? RecordsPerRequest { get; set; }
        public string SortBy { get; set; }
    }
}
