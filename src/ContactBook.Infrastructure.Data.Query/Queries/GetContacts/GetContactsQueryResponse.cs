using System.Collections.Generic;
using Newtonsoft.Json;

namespace ContactBook.Infrastructure.Data.Query.Queries.GetContacts {
    public sealed class GetContactsQueryResponse {
        [JsonProperty ("message")]
        public string Message { get; set; }

        [JsonProperty ("success")]
        public bool Success { get; set; }

        [JsonProperty ("data")]
        public IEnumerable<Contact> Contacts { get; set; }
    }
    public class Contact {

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