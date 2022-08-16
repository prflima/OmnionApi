using Omnion.Repository.Entities;
using System.Collections.Generic;

namespace Omnion.Business.Interfaces
{
    public interface IClienteService
    {
        bool CadastrarCliente(Cliente cliente, string conexao);
        bool AtualizarCliente(Cliente cliente, int idCliente, string conexao);
        bool DeletarCliente(int idCliente, string conexao);
        Cliente ObterDadosCliente(int idCliente, string conexao);
        List<Cliente> ObterDadosTodosClientes(string conexao);
        int AlterarPontosCliente(int saldoNovo, int idCliente, string conexao);
    }
}
