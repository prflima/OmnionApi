using Omnion.Repository.Entities;
using System.Collections.Generic;

namespace Omnion.Repository.Interfaces
{
    public interface IClienteRepository
    {
        List<Cliente> ObterTodosClientes(string conexao);
        Cliente ObterDadosCliente(int idCliente, string conexao);
        int CadastrarCliente(Cliente cliente, string conexao);
        bool AtualizarCliente(Cliente cliente, string conexao);
        bool DeletarCliente(int idCliente, string conexao);
        bool ValidarClienteExiste(string cpf, string conexao);
    }
}
