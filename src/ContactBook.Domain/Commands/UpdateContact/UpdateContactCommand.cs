using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Commands.UpdateContact {
    public sealed class UpdateContactCommand : IRequest<UpdateContactCommandResponse> {

        [JsonIgnore]
        public string Token { get; set; }

        [JsonProperty ("name")]
        public string Name { get; set; }

        [JsonProperty ("email")]
        public string Email { get; set; }

        [JsonProperty ("governmentId")]
        public string GovernmentId { get; set; }

        [JsonProperty ("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty ("address")]
        public string Address { get; set; }

        [JsonProperty ("image")]
        public string Image { get; set; }
    }
}