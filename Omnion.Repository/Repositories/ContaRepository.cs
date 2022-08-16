using Dapper;
using Omnion.Repository.Interfaces;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Omnion.Repository.Repositories
{
    public class ContaRepository : BaseRepository, IContaRepository
    {
        public bool AlterarPontosCliente(int pontos, int idCliente, string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    string update = @"UPDATE Conta
                                      SET SaldoPontos = @NovaPontuacao
                                      WHERE ClienteId = @IdCliente";

                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("NovaPontuacao", pontos);
                    Parameters.Add("IdCliente", idCliente);

                    int result = connection.Execute(update, Parameters);
                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool CriarContaCliente(int pontos, int idCliente , string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    string insert = @"INSERT INTO Conta(ClienteId, SaldoPontos)
                                      VALUES(@IdCliente, @Pontos)";

                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("IdCliente", idCliente);
                    Parameters.Add("Pontos", pontos);

                    int result = connection.Execute(insert, Parameters);
                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int VerificarPontuacaoCliente(int idCliente, string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    string select = @"SELECT SaldoPontos
                                      FROM Conta
                                      WHERE ClienteId = @IdCliente";

                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("IdCliente", idCliente);

                    return connection.Query<int>(select, Parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
