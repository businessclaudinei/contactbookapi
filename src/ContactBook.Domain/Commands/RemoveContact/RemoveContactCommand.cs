using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Domain.Commands.RemoveContact {
    public sealed class RemoveContactCommand : IRequest<RemoveContactCommandResponse> {

        [JsonIgnore]
        public string Token { get; set; }

        [JsonProperty ("email")]
        public string Email { get; set; }
    }
}