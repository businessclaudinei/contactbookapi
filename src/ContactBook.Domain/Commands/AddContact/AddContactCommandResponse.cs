using Newtonsoft.Json;

namespace ContactBook.Commands.AddContact {
    public sealed class AddContactCommandResponse {
        [JsonProperty ("message")]
        public string Message { get; set; }

        [JsonProperty ("success")]
        public bool Success { get; set; }
    }
}