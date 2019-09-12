using System;
using MediatR;

namespace ContactBook.Infrastructure.Data.Query.Queries.Get
{
    public sealed class Query:IRequest<Response>
    {
        public int Value { get; set; }
    }
}
