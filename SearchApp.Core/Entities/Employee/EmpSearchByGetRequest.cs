namespace SearchApp.Domain
{
    public class EmpSearchByGetRequest
    {
        public string FilterKey;
        public string FilterValue;
        public string SortBy;
        public string SortOrder;
        public int PageNumber;
        public int PageSize;
        public string UserId;
    }
}
