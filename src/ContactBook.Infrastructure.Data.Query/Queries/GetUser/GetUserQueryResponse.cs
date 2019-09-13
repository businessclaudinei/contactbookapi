using System.Collections.Generic;
using Newtonsoft.Json;

namespace ContactBook.Infrastructure.Data.Query.Queries.GetUser {
    public sealed class GetUserQueryResponse {

        [JsonProperty ("message")]
        public string Message { get; set; }

        [JsonProperty ("success")]
        public bool Success { get; set; }

        [JsonProperty ("data")]
        public User User { get; set; }
    }
    public class User {

        [JsonProperty ("name")]
        public string Name { get; set; }

        [JsonProperty ("email")]
        public string Email { get; set; }

        [JsonProperty ("password")]
        public string Password { get; set; }

        [JsonProperty ("token")]
        public string Token { get; set; }

        [JsonProperty ("role")]
        public string Role { get; set; }

        [JsonProperty ("image")]
        public string Image { get; set; }
    }
}