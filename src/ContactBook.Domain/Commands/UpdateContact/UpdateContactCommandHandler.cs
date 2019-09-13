using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContactBook.Commands.UpdateContact;
using ContactBook.Domain.Commands.AddUser;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;
using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Domain.UpdateContact {
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, UpdateContactCommandResponse> {
        private readonly IResponseCacheService _responseCacheService;
        public UpdateContactCommandHandler (IResponseCacheService responseCacheService) {
            _responseCacheService = responseCacheService;
        }

        public async Task<UpdateContactCommandResponse> Handle (UpdateContactCommand command, CancellationToken cancellation) {
            var user = new AddUserCommand ();
            try {
                var data = await _responseCacheService.ManageTokenAsync (command.Token);
                if (data != null) {
                    user = JsonConvert.DeserializeObject<AddUserCommand> (data);
                }
            } catch (Exception ex) {
                return new UpdateContactCommandResponse () { Message = "Voce precisa logar!", Success = false };
            }

            var contacts = new List<UpdateContactCommand> ();
            try {
                var cachedValue = await _responseCacheService.GetCachedResponseAsync (user.Email);

                if (cachedValue != null)
                    contacts = JsonConvert.DeserializeObject<List<UpdateContactCommand>> (cachedValue);
            } catch (Exception ex) {
                return new UpdateContactCommandResponse () { Message = "Erro ao atualizar o contato!", Success = false };
            }
            contacts = contacts.Where (x => !x.Email.Equals (command.Email)).ToList ();
            contacts.Add (command);

            var expiration = Convert.ToInt32 (Environment.GetEnvironmentVariable ("DATA_EXPIRATION_SECONDS"));
            expiration = expiration < 1 ? 900 : expiration;
            _responseCacheService.CacheResponseAsync (user.Email, contacts, new System.TimeSpan (0, 0, expiration)).ConfigureAwait (false);

            return new UpdateContactCommandResponse () { Message = "Usuario atualizado com sucesso!", Success = true };
        }
    }
}