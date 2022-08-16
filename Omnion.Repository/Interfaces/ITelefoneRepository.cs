using Omnion.Repository.Entities;
using System.Collections.Generic;

namespace Omnion.Repository.Interfaces
{
    public interface ITelefoneRepository
    {
        List<Telefone> ObterDadosTelefoneCliente(int idCliente, string conexao);
        bool CadastrarTelefonesCliente(Telefone telefone, int idCliente, string conexao);
        bool AtualizarTelefones(Telefone telefone, int idCliente, string conexao);
        Telefone ObterTelefonePorNumero(string telefone, string conexao);
    }
}
