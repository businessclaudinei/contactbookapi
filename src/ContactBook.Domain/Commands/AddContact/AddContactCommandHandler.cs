using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContactBook.Commands.AddContact;
using ContactBook.Domain.Commands.AddUser;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;
using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Domain.AddContact {
    public class AddContactCommandHandler : IRequestHandler<AddContactCommand, AddContactCommandResponse> {
        private readonly IResponseCacheService _responseCacheService;
        public AddContactCommandHandler (IResponseCacheService responseCacheService) {
            _responseCacheService = responseCacheService;
        }

        public async Task<AddContactCommandResponse> Handle (AddContactCommand command, CancellationToken cancellation) {
            var user = new AddUserCommand ();
            try {
                var data = await _responseCacheService.ManageTokenAsync (command.Token);
                if (data != null) {
                    user = JsonConvert.DeserializeObject<AddUserCommand> (data);
                }
            } catch (Exception ex) {
                return new AddContactCommandResponse () { Message = "Você precisa logar!", Success = false };
            }

            var contacts = new List<AddContactCommand> ();
            try {
                var cachedValue = await _responseCacheService.GetCachedResponseAsync (user.Email);

                if (cachedValue != null)
                    contacts = JsonConvert.DeserializeObject<List<AddContactCommand>> (cachedValue);
            } catch (Exception ex) {
                return new AddContactCommandResponse () { Message = "Erro ao adicionar o contato!", Success = false };
            }

            contacts.Add (command);

            var expiration = Convert.ToInt32 (Environment.GetEnvironmentVariable ("DATA_EXPIRATION_SECONDS"));
            expiration = expiration < 1 ? 900 : expiration;

            _responseCacheService.CacheResponseAsync (user.Email, contacts, new TimeSpan (0, 0, expiration)).ConfigureAwait (false);

            return new AddContactCommandResponse () { Message = "Usuario cadastrado com sucesso!", Success = true };
        }
    }
}