using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContactBook.Domain.Commands.AddUser;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;
using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Domain.Commands.UpdateUser {
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse> {
        private readonly IResponseCacheService _responseCacheService;
        public UpdateUserCommandHandler (IResponseCacheService responseCacheService) {
            _responseCacheService = responseCacheService;
        }

        public async Task<UpdateUserCommandResponse> Handle (UpdateUserCommand command, CancellationToken cancellation) {

            var sessionUser = new AddUserCommand ();
            try {
                var data = await _responseCacheService.ManageTokenAsync (command.Token);
                if (data != null) {
                    sessionUser = JsonConvert.DeserializeObject<AddUserCommand> (data);
                }
            } catch (Exception ex) {
                return new UpdateUserCommandResponse () { Message = "Você precisa logar!", Success = false };
            }

            var users = new List<AddUserCommand> ();
            try {
                var cachedValue = await _responseCacheService.GetCachedResponseAsync ("users");
                if (cachedValue != null)
                    users = JsonConvert.DeserializeObject<List<AddUserCommand>> (cachedValue);
            } catch (Exception ex) {
                return new UpdateUserCommandResponse () { Message = "Erro ao alterar senha", Success = false };
            }

            var user = users.FirstOrDefault (x => x.Email.Equals (command.Email));

            if (user != null) {
                if (sessionUser.Email.Equals (user.Email))
                    user.Password = command.Password;
                else
                    return new UpdateUserCommandResponse () { Message = "Alteração não autorizada!", Success = false };
            } else {
                return new UpdateUserCommandResponse () { Message = "Email não cadastrado", Success = false };
            }

            var expiration = Convert.ToInt32 (Environment.GetEnvironmentVariable ("DATA_EXPIRATION_SECONDS"));
            expiration = expiration < 1 ? 900 : expiration;
            _responseCacheService.CacheResponseAsync ("users", users, new System.TimeSpan (0, 0, expiration)).ConfigureAwait (false);

            return new UpdateUserCommandResponse () { Message = "Senha alterada com sucesso!", Success = true };
        }
    }
}