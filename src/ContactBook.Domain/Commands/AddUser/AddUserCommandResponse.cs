using Newtonsoft.Json;

namespace ContactBook.Commands.AddUser {
    public sealed class AddUserCommandResponse {
        [JsonProperty ("message")]
        public string Message { get; set; }

        [JsonProperty ("success")]
        public bool Success { get; set; }
    }
}