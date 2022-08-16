using Microsoft.AspNetCore.Mvc;
using Omnion.Business.Interfaces;
using Omnion.Business.Notificacoes;
using OmnionAPI.Configuration;
using System.Linq;

namespace OmnionAPI.Controllers
{
    public abstract class MainController : ControllerBase
    {
        private readonly INotificador _notificador;
        private readonly string _connectionStringOmnion;

        public MainController(INotificador notificador)
        {
            _notificador = notificador;
            _connectionStringOmnion = DefinicoesConfiguracao.ObterConexaoBancoDeDados("OmnionDatabase");
        }


        protected string ConnectionString => _connectionStringOmnion;
        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacao().Select(nt => nt.Mensagem)
            });
        }

        protected void NotificarErro(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }
    }
}
