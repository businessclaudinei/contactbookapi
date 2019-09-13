using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Domain.Commands.UpdateUser {
    public sealed class UpdateUserCommand : IRequest<UpdateUserCommandResponse> {
        [JsonIgnore]
        public string Token { get; set; }

        [JsonProperty ("email")]
        public string Email { get; set; }

        [JsonProperty ("password")]
        public string Password { get; set; }
    }
}