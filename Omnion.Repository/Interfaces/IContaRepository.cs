namespace Omnion.Repository.Interfaces
{
    public interface IContaRepository
    {
        int VerificarPontuacaoCliente(int idCliente, string conexao);
        bool CriarContaCliente(int pontos, int idCliente, string conexao);

        bool AlterarPontosCliente(int pontos, int idCliente, string conexao);
    }
}
