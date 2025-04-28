using System.ComponentModel.DataAnnotations;

namespace SearchApp.Core
{
    public class SearchHistory
    {
        [Key]
        public int Id { get; set; }
        public string Department { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal? Salary { get; set; } = null!;
        public string FromDate { get; set; } = null!;
        public string ToDate { get; set; } = null!;
        public int RecordsPerRequest { get; set; }
        public string SortBy { get; set; } = null!;
        public string SortOrder { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime SearchDate { get; set; }
        public int ResultCount { get; set; }
    }
}
