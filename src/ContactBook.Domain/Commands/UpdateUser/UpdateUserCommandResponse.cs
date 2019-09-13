using Newtonsoft.Json;

namespace ContactBook.Domain.Commands.UpdateUser {
    public sealed class UpdateUserCommandResponse {
        [JsonProperty ("message")]
        public string Message { get; set; }

        [JsonProperty ("success")]
        public bool Success { get; set; }
    }
}