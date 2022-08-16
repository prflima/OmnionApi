using DevIO.Business.Models.Validations.Documentos;
using FluentValidation;
using Omnion.Repository.Entities;

namespace Omnion.Business.Validations
{
    public class ClienteValidation : AbstractValidator<Cliente>
    {
        public ClienteValidation()
        {
            RuleFor(cl => cl.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(1, 100)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            RuleFor(cl => cl.CPF.Length).Equal(CpfValidacao.TamanhoCpf)
                .WithMessage("O Campo precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}");
            RuleFor(cl => CpfValidacao.Validar(cl.CPF)).Equal(true)
                .WithMessage("O CPF fornecido é inválido.");
        }
    }
}
