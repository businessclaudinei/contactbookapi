using System.Collections.Generic;
public sealed class ErrorResponse
{
    public IEnumerable<ErrorModel> Errors { get; set; }
}
