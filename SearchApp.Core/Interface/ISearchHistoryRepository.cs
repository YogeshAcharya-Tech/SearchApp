namespace SearchApp.Domain
{
    public interface ISearchHistoryRepository
    {
        Task<IEnumerable<SearchHistory>> GetSearchHistory(string userId);
    }
}
