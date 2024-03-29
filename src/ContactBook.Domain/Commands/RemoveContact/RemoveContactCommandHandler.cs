using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContactBook.Commands.AddContact;
using ContactBook.Domain.Commands.AddUser;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;
using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Domain.Commands.RemoveContact {
    public class RemoveContactCommandHandler : IRequestHandler<RemoveContactCommand, RemoveContactCommandResponse> {
        private readonly IResponseCacheService _responseCacheService;
        public RemoveContactCommandHandler (IResponseCacheService responseCacheService) {
            _responseCacheService = responseCacheService;
        }

        public async Task<RemoveContactCommandResponse> Handle (RemoveContactCommand command, CancellationToken cancellation) {
            var user = new AddUserCommand ();
            try {
                var data = await _responseCacheService.ManageTokenAsync (command.Token);
                if (data != null) {
                    user = JsonConvert.DeserializeObject<AddUserCommand> (data);
                }
            } catch (Exception ex) {
                return new RemoveContactCommandResponse () { Message = "Voce precisa logar!", Success = false };
            }

            var contacts = new List<AddContactCommand> ();
            try {
                var cachedValue = await _responseCacheService.GetCachedResponseAsync (user.Email);

                if (cachedValue != null)
                    contacts = JsonConvert.DeserializeObject<List<AddContactCommand>> (cachedValue);
            } catch (Exception ex) {
                return new RemoveContactCommandResponse () { Message = "Erro ao remover contato!", Success = false };
            }
            contacts = contacts.Where (x => !x.Email.Equals (command.Email)).ToList ();

            var expiration = Convert.ToInt32 (Environment.GetEnvironmentVariable ("DATA_EXPIRATION_SECONDS"));
            expiration = expiration < 1 ? 900 : expiration;

            _responseCacheService.CacheResponseAsync (user.Email, contacts, new System.TimeSpan (0, 0, expiration)).ConfigureAwait (false);

            return new RemoveContactCommandResponse () { Message = "Usuario removido com sucesso!", Success = true };
        }
    }
}