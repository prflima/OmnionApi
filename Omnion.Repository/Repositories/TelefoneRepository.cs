using Dapper;
using Omnion.Repository.Entities;
using Omnion.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Omnion.Repository.Repositories
{
    public class TelefoneRepository : BaseRepository, ITelefoneRepository
    {
        public bool AtualizarTelefones(Telefone telefone, int idCliente, string conexao)
        {

            using (var connection = new SqlConnection(conexao))
            {
                connection.Open();

                try
                {
                    string update = @"UPDATE Telefone
                                          SET DDD = @DDD, Numero = @Numero
                                          WHERE ClienteId = @IdCliente";

                    Parameters.Clear();
                    Parameters.Add("DDD", telefone.DDD);
                    Parameters.Add("Numero", telefone.Numero);
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


        public bool CadastrarTelefonesCliente(Telefone telefone, int idCliente, string conexao)
        {
            using (var connection = new SqlConnection(conexao))
            {
                connection.Open();

                try
                {
                    string insert = @"INSERT INTO Telefone(ClienteId, DDD, Numero)
                                          VALUES(@IdCliente, @DDD, @Numero)";

                    Parameters.Clear();
                    Parameters.Add("DDD", telefone.DDD);
                    Parameters.Add("Numero", telefone.Numero);
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


        public List<Telefone> ObterDadosTelefoneCliente(int idCliente, string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    string select = @"SELECT DDD, Numero
                                      FROM Telefone
                                      WHERE ClienteId = @IdCliente";

                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("IdCliente", idCliente);

                    return connection.Query<Telefone>(select, Parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Telefone ObterTelefonePorNumero(string telefone, string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    string selectNumero = @"SELECT Id, ClienteId, DDD, Numero 
                                            FROM Telefone
                                            WHERE Numero = @Numero";

                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("Numero", telefone);

                    return connection.Query<Telefone>(selectNumero, Parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
