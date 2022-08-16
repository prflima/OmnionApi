using Dapper;
using Omnion.Repository.Entities;
using Omnion.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Omnion.Repository.Repositories
{
    public class ClienteRepository : BaseRepository, IClienteRepository
    {
        public bool AtualizarCliente(Cliente cliente, string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("Nome", cliente.Nome);
                    Parameters.Add("DataAlteracao", DateTime.Now);
                    Parameters.Add("IdCliente", cliente.Id);

                    string update = @"UPDATE Cliente
                                      SET Nome = @Nome, DataAlteracao = @DataAlteracao
                                      WHERE Id = @IdCliente";

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

        public int CadastrarCliente(Cliente cliente, string conexao)
        {
            try
            {
                using(var connection = new SqlConnection(conexao))
                {
                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("NomeCliente", cliente.Nome);
                    Parameters.Add("CPF", cliente.CPF);
                    Parameters.Add("DataCadastro", DateTime.Now);

                    string insert = @"INSERT INTO Cliente(Nome, CPF, DataCadastro)
                                      VALUES (@NomeCliente, @CPF, @DataCadastro);
                                      SELECT CAST(SCOPE_IDENTITY() AS INT) AS NovoIdCliente";

                    var novoId = connection.Query(insert, Parameters).Single();
                    return Convert.ToInt32(novoId.NovoIdCliente);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeletarCliente(int idCliente, string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("IdCliente", idCliente);

                    string delete = @"DELETE FROM Cliente
                                      WHERE Id = @IdCliente";

                   int result = connection.Execute(delete, Parameters);
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

        public Cliente ObterDadosCliente(int idCliente, string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("IdCliente", idCliente);

                    string select = @"SELECT Id, Nome, CPF, DataCadastro, DataAlteracao
                                      FROM Cliente 
                                      WHERE Id = @IdCliente";

                    return connection.Query<Cliente>(select, Parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Cliente> ObterTodosClientes(string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    string select = @"SELECT Id, Nome, CPF, DataCadastro, DataAlteracao
                                      FROM Cliente ";

                    connection.Open();

                    return connection.Query<Cliente>(select).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ValidarClienteExiste(string cpf, string conexao)
        {
            try
            {
                using (var connection = new SqlConnection(conexao))
                {
                    connection.Open();
                    Parameters.Clear();
                    Parameters.Add("CPF", cpf);

                    string validar = @"SELECT Id
                                       FROM Cliente
                                       WHERE CPF = @CPF;";

                    int id = connection.Query<int>(validar, Parameters).FirstOrDefault();
                    if (id != 0)
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
    }
}
