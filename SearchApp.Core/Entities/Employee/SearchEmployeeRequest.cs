namespace SearchApp.Core.Entities
{
    public class SearchEmployeeRequest
    {
        public string Department { get; set; }
        public string Name { get; set; }
        public decimal? Salary { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int RecordsPerRequest { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string UserId { get; set; } // Not used for filter used to save search performed by which user
    }
}
