using Newtonsoft.Json;

namespace ContactBook.Domain.Commands.RemoveContact {
    public sealed class RemoveContactCommandResponse {
        [JsonProperty ("message")]
        public string Message { get; set; }

        [JsonProperty ("success")]
        public bool Success { get; set; }
    }
}