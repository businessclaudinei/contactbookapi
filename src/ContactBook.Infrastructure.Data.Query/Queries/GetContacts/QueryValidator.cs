using FluentValidation;

namespace ContactBook.Infrastructure.Data.Query.Queries.GetContacts {
    public sealed class GetContactsQueryValidator : AbstractValidator<GetContactsQuery> {
        public GetContactsQueryValidator () {
            // RuleFor (query => query.userId)
            //     .NotEmpty ().WithMessage ("Id esta vazio!");
        }
    }
}