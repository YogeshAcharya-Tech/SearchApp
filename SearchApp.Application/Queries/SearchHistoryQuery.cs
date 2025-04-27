using MediatR;
using SearchApp.Core.Entities;

namespace SearchApp.Application.Queries
{
    public class SearchHistoryQuery : IRequest<List<SearchHistory>>
    {
        public string UserId { get; set; }
        public SearchHistoryQuery(string userId)
        {
            UserId = userId;
        }
    }
}
