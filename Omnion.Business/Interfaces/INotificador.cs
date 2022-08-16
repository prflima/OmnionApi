using Omnion.Business.Notificacoes;
using System.Collections.Generic;

namespace Omnion.Business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacao();
        void Handle(Notificacao notificacao);
    }
}
