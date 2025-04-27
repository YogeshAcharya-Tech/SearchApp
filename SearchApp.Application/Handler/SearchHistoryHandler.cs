using MediatR;
using SearchApp.Application.Queries;
using SearchApp.Core;
using SearchApp.Core.Entities;

namespace SearchApp.Application.Handler
{
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
