using FluentValidation;
using FluentValidation.Results;
using Omnion.Business.Interfaces;
using Omnion.Business.Notificacoes;
using Omnion.Repository.Entities;

namespace Omnion.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        protected BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            try
            {
                var validator = validacao.Validate(entidade);

                if (validator.IsValid) return true;

                Notificar(validator);

                return false;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
    }
}
