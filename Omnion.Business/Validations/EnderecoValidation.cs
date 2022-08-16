using FluentValidation;
using Omnion.Repository.Entities;

namespace Omnion.Business.Validations
{
    class EnderecoValidation : AbstractValidator<Endereco>
    {
        public EnderecoValidation()
        {
            RuleFor(en => en.Rua)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa fornecido.")
                .Length(1, 150)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(cl => cl.CEP)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa fornecido.")
                .Length(8)
                .WithMessage("O campo {PropertyName} precisa ter {MaxLength} caracteres");
        }
    }
}
