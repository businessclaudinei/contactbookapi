using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;
using Newtonsoft.Json;

namespace ContactBook.Infrastructure.Data.Query.Queries.Get
{
    public class QueryHandler:IRequestHandler<Query, Response>
    {
        private readonly IMapper _mapper;
        private readonly IResponseCacheService _responseCacheService;
        public QueryHandler(IMapper mapper, IResponseCacheService responseCacheService)
        {
            _mapper = mapper;
            _responseCacheService = responseCacheService;
        }

        public async Task<Response> Handle(Query query, CancellationToken cancellation)
        {
            var cachedValue = await _responseCacheService.GetCachedResponseAsync("value");
            if(cachedValue != null)
            {
                var cachedResponse = JsonConvert.DeserializeObject<Response>(cachedValue);
                cachedResponse.Value += 1;
                return cachedResponse;
            }

            var response = _mapper.Map<Response>(query);
            response.Value *= 3;

            _responseCacheService.CacheResponseAsync("value", response, new System.TimeSpan(0,0,20)).ConfigureAwait(false);
            
            return response;
        }
    }
}
