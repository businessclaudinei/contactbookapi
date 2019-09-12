using FluentValidation;

namespace ContactBook.Infrastructure.Data.Query.Queries.Get
{
    public sealed class QueryValidator:AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(query=>query.Value)
            .NotEmpty().WithMessage("It is empty!");
        }
    }
}
