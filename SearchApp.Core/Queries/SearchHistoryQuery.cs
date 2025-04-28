using MediatR;

namespace SearchApp.Core
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
