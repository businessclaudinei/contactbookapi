using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContactBook.Infrastructure.Data.Query.Queries.GetUser;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;
using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Infrastructure.Data.Query.Queries.GetContacts {
    public class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, GetContactsQueryResponse> {
        private readonly IMapper _mapper;
        private readonly IResponseCacheService _responseCacheService;
        public GetContactsQueryHandler (IMapper mapper, IResponseCacheService responseCacheService) {
            _mapper = mapper;
            _responseCacheService = responseCacheService;
        }

        public async Task<GetContactsQueryResponse> Handle (GetContactsQuery query, CancellationToken cancellation) {
            var user = new User ();
            try {
                var data = await _responseCacheService.GetCachedResponseAsync (query.Token);
                if (data != null) {
                    user = JsonConvert.DeserializeObject<User> (data);
                }
            } catch (Exception ex) {
                return new GetContactsQueryResponse () { };
            }

            var response = new GetContactsQueryResponse ();
            try {
                var cachedValue = await _responseCacheService.GetCachedResponseAsync (user.Email);
                if (cachedValue != null) {
                    var cachedResponse = JsonConvert.DeserializeObject<IEnumerable<Contact>> (cachedValue);
                    response.Contacts = cachedResponse;
                    response.Success = true;
                }
            } catch (Exception ex) {
                throw ex;
            }

            return response;
        }
    }
}