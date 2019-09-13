using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Commands.AddUser {
    public sealed class AddUserCommand : IRequest<AddUserCommandResponse> {

        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("name")]
        public string Name { get; set; }

        [JsonProperty ("email")]
        public string Email { get; set; }

        [JsonProperty ("password")]
        public string Password { get; set; }

        [JsonProperty ("image")]
        public string Image { get; set; }
    }
}