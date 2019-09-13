using FluentValidation;

namespace ContactBook.Infrastructure.Data.Query.Queries.GetUser {
    public sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery> {
        public GetUserQueryValidator () {
            // RuleFor (query => query.userId)
            //     .NotEmpty ().WithMessage ("Id esta vazio!");
        }
    }
}