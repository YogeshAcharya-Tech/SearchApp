namespace SearchApp.Core.Entities
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public string SearchTerm { get; set; }
        public string? FilterTerm { get; set; }
        public string UserId { get; set; }
        public DateTime SearchDate { get; set; }
        public int ResultCount { get; set; }
    }
}
