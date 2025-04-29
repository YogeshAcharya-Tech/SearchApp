using MediatR;
using SearchApp.Domain;

namespace SearchApp.Application
{
    public class SearchHistoryQuery : IRequest<List<SearchHistory>>
    {
        public string UserId { get; set; }
        public SearchHistoryQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class SearchHistoryHandler : IRequestHandler<SearchHistoryQuery, List<SearchHistory>>
    {
        private readonly ISearchHistoryRepository _searchHistoryRepository;

        public SearchHistoryHandler(ISearchHistoryRepository searchHistoryRepository)
        {
            _searchHistoryRepository = searchHistoryRepository;
        }

        public async Task<List<SearchHistory>> Handle(SearchHistoryQuery request, CancellationToken cancellationToken)
        {
            return (List<SearchHistory>)await _searchHistoryRepository.GetSearchHistory(request.UserId);
        }
    }
}
