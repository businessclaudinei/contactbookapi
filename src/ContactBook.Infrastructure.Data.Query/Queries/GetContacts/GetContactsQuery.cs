using MediatR;
using Newtonsoft.Json;

namespace ContactBook.Infrastructure.Data.Query.Queries.GetContacts {
    public sealed class GetContactsQuery : IRequest<GetContactsQueryResponse> {
        [JsonIgnore]
        public string Token { get; set; }
    }
}