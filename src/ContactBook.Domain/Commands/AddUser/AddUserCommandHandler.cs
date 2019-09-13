using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;
using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Domain.Commands.AddUser {
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, AddUserCommandResponse> {
        private readonly IResponseCacheService _responseCacheService;
        public AddUserCommandHandler (IResponseCacheService responseCacheService) {
            _responseCacheService = responseCacheService;
        }

        public async Task<AddUserCommandResponse> Handle (AddUserCommand command, CancellationToken cancellation) {

            var users = new List<AddUserCommand> ();
            try {
                var cachedValue = await _responseCacheService.GetCachedResponseAsync ("users");
                if (cachedValue != null)
                    users = JsonConvert.DeserializeObject<List<AddUserCommand>> (cachedValue);
            } catch (Exception ex) {
                return new AddUserCommandResponse () { Message = "Erro ao cadastrar usuario", Success = false };
            }

            command.Image = "https://picsum.photos/300/";
            if (!users.Any (x => x.Email.Equals (command.Email)))
                users.Add (command);
            else
                return new AddUserCommandResponse () { Message = "Usuario ja cadastrado com este email.", Success = false };

            var expiration = Convert.ToInt32 (Environment.GetEnvironmentVariable ("DATA_EXPIRATION_SECONDS"));
            expiration = expiration < 1 ? 900 : expiration;
            _responseCacheService.CacheResponseAsync ("users", users, new TimeSpan (0, 0, expiration)).ConfigureAwait (false);

            return new AddUserCommandResponse () { Message = "Usuario Cadastrado com sucesso!", Success = true };
        }
    }
}