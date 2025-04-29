namespace SearchApp.Domain
{
    public class SearchEmployeeRequest
    {
        public string Department { get; set; }
        public string Name { get; set; }
        public decimal? Salary { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PageNumber { get; set; } = 1; // Pagination Requested Page No
        public int PageSize { get; set; } = 10; // Pagination Records Per Page
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string UserId { get; set; } // Not used for filter used to save search performed by which user
    }
}
