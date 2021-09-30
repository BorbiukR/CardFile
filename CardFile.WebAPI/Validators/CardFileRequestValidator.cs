using CardFile.WebAPI.Contracts.Request;
using FluentValidation;

namespace CardFile.WebAPI.Validators
{
    public class CardFileRequestValidator : AbstractValidator<CardFileRequest>
    {
        public CardFileRequestValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(400)
                .MinimumLength(10);

            RuleFor(x => x.Language)
                .NotEmpty()
                .MaximumLength(40)
                .MinimumLength(3);
        }
    }
}