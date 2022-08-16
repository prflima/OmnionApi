using Omnion.Repository.Entities;
using System.Collections.Generic;

namespace Omnion.Repository.Interfaces
{
    public interface IEnderecoRepository
    {
        List<Endereco> ObterEnderecosCliente(int idCliente, string conexao);
        bool CadastrarEnderecosCliente(Endereco endereco, int idCliente, string conexao);
        bool AtualizarEnderecosCliente(Endereco endereco, int idCliente, string conexao);
    }
}
