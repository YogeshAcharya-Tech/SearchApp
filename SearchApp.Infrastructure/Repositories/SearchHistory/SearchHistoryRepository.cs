using Microsoft.EntityFrameworkCore;
using SearchApp.Core;
using SearchApp.Core.Entities;
using SearchApp.Infrastructure.Data;

namespace SearchApp.Infrastructure.Repositories
{
    public class SearchHistoryRepository(AppDbContext dbContext) : ISearchHistoryRepository
    {
        public async Task<IEnumerable<SearchHistory>> GetSearchHistory(string userId)
        {
            var result = await dbContext.SearchHistory.Where(s => s.UserId == userId).OrderByDescending(s => s.SearchDate).ToListAsync();
            return result;
        }
    }
}
