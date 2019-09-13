using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Infrastructure.Data.Query.Queries.GetUser {
    public sealed class GetUserQuery : IRequest<GetUserQueryResponse> {
        [JsonProperty ("email")]
        public string Email { get; set; }

        [JsonProperty ("password")]
        public string Password { get; set; }
    }
}