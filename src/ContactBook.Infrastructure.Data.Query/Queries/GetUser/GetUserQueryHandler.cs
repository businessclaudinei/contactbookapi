using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;
using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Infrastructure.Data.Query.Queries.GetUser {
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResponse> {
        private readonly IMapper _mapper;
        private readonly IResponseCacheService _responseCacheService;
        public GetUserQueryHandler (IMapper mapper, IResponseCacheService responseCacheService) {
            _mapper = mapper;
            _responseCacheService = responseCacheService;
        }

        public async Task<GetUserQueryResponse> Handle (GetUserQuery query, CancellationToken cancellation) {
            var users = new List<User> ();
            try {
                var cachedValue = await _responseCacheService.GetCachedResponseAsync ("users");
                if (cachedValue != null)
                    users = JsonConvert.DeserializeObject<List<User>> (cachedValue);
            } catch (Exception ex) {
                return new GetUserQueryResponse () { Message = "Ocorreu um erro inesperado.", Success = false };
            }

            var loggedUser = users.FirstOrDefault (x => x.Email.Equals (query.Email) && x.Password.Equals (query.Password));

            if (loggedUser == null)
                return new GetUserQueryResponse () { Message = "Senha ou email invalido", Success = false };

            try {
                loggedUser.Token = WindowsIdentity.GetCurrent ().Token.ToString ();
            } catch {
                loggedUser.Token = System.Convert.ToBase64String (System.Text.Encoding.Default.GetBytes (loggedUser.Email + loggedUser.Name));
            }
            loggedUser.Role = "user";

            _responseCacheService.ManageTokenAsync (loggedUser.Token, loggedUser).ConfigureAwait (false);

            return new GetUserQueryResponse () { Message = "", Success = true, User = loggedUser };;
        }
    }
}