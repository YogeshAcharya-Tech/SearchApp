namespace SearchApp.Core
{
    public interface ISearchHistoryRepository
    {
        Task<IEnumerable<SearchHistory>> GetSearchHistory(string userId);
    }
}
