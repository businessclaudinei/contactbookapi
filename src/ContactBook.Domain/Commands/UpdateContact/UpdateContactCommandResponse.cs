using Newtonsoft.Json;

namespace ContactBook.Commands.UpdateContact {
    public sealed class UpdateContactCommandResponse {
        [JsonProperty ("message")]
        public string Message { get; set; }

        [JsonProperty ("success")]
        public bool Success { get; set; }
    }
}