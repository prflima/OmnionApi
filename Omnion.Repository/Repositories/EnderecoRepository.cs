using Dapper;
using Omnion.Repository.Entities;
using Omnion.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Omnion.Repository.Repositories
{
    public class EnderecoRepository : BaseRepository, IEnderecoRepository
    {
        public bool AtualizarEnderecosCliente(Endereco endereco, int idCliente, string conexao)
        {
            using (var connection = new SqlConnection(conexao))
            {
                connection.Open();

                try
                {
                    string update = @"UPDATE Endereco
                                          SET Rua = @Rua, Numero = @Numero, CEP = @CEP
                                          WHERE ClienteId = @IdCliente;";

                    Parameters.Clear();
                    Parameters.Add("Rua", endereco.Rua);
                    Parameters.Add("Numero", endereco.Numero);
                    Parameters.Add("CEP", endereco.CEP);
                    Parameters.Add("IdCliente", idCliente);

                    connection.Execute(update, Parameters);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }

        public bool CadastrarEnderecosCliente(Endereco endereco, int idCliente, string conexao)
        {
            using (var connection = new SqlConnection(conexao))
            {
                connection.Open();
                try
                {
                    string insert = @"INSERT INTO Endereco(ClienteId, Rua, Numero, CEP)
                                      VALUES(@IdCliente, @Rua, @Numero, @CEP)";


                    Parameters.Clear();
                    Parameters.Add("Rua", endereco.Rua);
                    Parameters.Add("Numero", endereco.Numero);
                    Parameters.Add("CEP", endereco.CEP);
                    Parameters.Add("IdCliente", idCliente);

                    connection.Execute(insert, Parameters);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public List<Endereco> ObterEnderecosCliente(int idCliente, string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    string select = @"SELECT Rua, Numero, CEP
                                      FROM Endereco
                                      WHERE ClienteId = @IdCliente";

                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("IdCliente", idCliente);

                    return connection.Query<Endereco>(select, Parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
