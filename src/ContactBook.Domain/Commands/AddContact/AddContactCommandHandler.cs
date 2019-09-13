using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContactBook.Commands.AddContact;
using ContactBook.Commands.AddUser;
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
                var data = await _responseCacheService.GetCachedResponseAsync (command.Token);
                if (data != null) {
                    user = JsonConvert.DeserializeObject<AddUserCommand> (data);
                }
            } catch (Exception ex) {
                return new AddContactCommandResponse () { };
            }

            var contacts = new List<AddContactCommand> ();
            var cachedValue = await _responseCacheService.GetCachedResponseAsync (user.Email);

            if (cachedValue != null)
                contacts = JsonConvert.DeserializeObject<List<AddContactCommand>> (cachedValue);

            contacts.Add (command);

            _responseCacheService.CacheResponseAsync (user.Email, contacts, new System.TimeSpan (0, 0, 600)).ConfigureAwait (false);

            return new AddContactCommandResponse ();
        }
    }
}