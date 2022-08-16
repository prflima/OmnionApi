using Omnion.Business.Interfaces;
using Omnion.Business.Validations;
using Omnion.Repository.Entities;
using Omnion.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace Omnion.Business.Services
{
    public class ClienteService : BaseService, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly ITelefoneRepository _telefoneRepository;
        private readonly IContaRepository _contaRepository;
        private int PontuacaoMaxima = 1000;

        public ClienteService(IClienteRepository clienteRepository,
                              IEnderecoRepository enderecoRepository,
                              ITelefoneRepository telefoneRepository,
                              IContaRepository contaRepository,
                              INotificador notificador) : base(notificador)
        {
            _clienteRepository = clienteRepository;
            _enderecoRepository = enderecoRepository;
            _telefoneRepository = telefoneRepository;
            _contaRepository = contaRepository;
        }

        public int AlterarPontosCliente(int saldoNovo, int idCliente, string conexao)
        {
            int saldoBanco = _contaRepository.VerificarPontuacaoCliente(idCliente, conexao);
            int novoSaldo = (saldoBanco + saldoNovo);
            if (saldoBanco != 0)
            {
                if (novoSaldo > PontuacaoMaxima)
                {
                    Notificar("O usuário não pode ter mais de mil pontos");
                    return novoSaldo;
                }

                if (novoSaldo < 0)
                {
                    Notificar($"Novo Saldo é inválido: {novoSaldo}");
                    return novoSaldo;
                }

                _contaRepository.AlterarPontosCliente(novoSaldo, idCliente, conexao);
            }
            else
            {
                if (novoSaldo > PontuacaoMaxima)
                {
                    Notificar("O usuário não pode ter mais de mil pontos");
                    return novoSaldo;
                }

                if (novoSaldo < 0)
                {
                    Notificar($"Novo Saldo é inválido: {novoSaldo}");
                    return novoSaldo;
                }

                _contaRepository.CriarContaCliente(novoSaldo, idCliente, conexao);
            }

            return novoSaldo;
        }

        public bool AtualizarCliente(Cliente cliente, int idCliente, string conexao)
        {
            if (!String.IsNullOrEmpty(cliente.CPF))
            {
                Notificar("Não é possível alterar o CPF");
                return false;
            }

            var clienteBanco = _clienteRepository.ObterDadosCliente(idCliente, conexao);
            if (clienteBanco == null)
            {
                Notificar("Esse Cliente não existe");
                return false;
            }
            _clienteRepository.AtualizarCliente(cliente, conexao);

            if (cliente.Enderecos.Count > 0)
            {
                foreach (var endereco in cliente.Enderecos)
                {
                    if (!ExecutarValidacao(new EnderecoValidation(), endereco))
                    {
                        return false;
                    }
                    _enderecoRepository.AtualizarEnderecosCliente(endereco, idCliente, conexao);
                }

            }

            if (cliente.Telefones.Count > 0)
            {
                foreach (var telefone in cliente.Telefones)
                {
                    var telExiste = _telefoneRepository.ObterTelefonePorNumero(telefone.Numero, conexao);

                    if (telExiste != null && clienteBanco.Id != telExiste.ClienteId)
                    {
                        Notificar($"Este número: {telExiste.DDD} {telExiste.Numero} já está cadastrado para outro usuário");
                        return false;
                    }
                    _telefoneRepository.AtualizarTelefones(telefone, idCliente, conexao);
                }

            }

            int saldoBanco = _contaRepository.VerificarPontuacaoCliente(clienteBanco.Id, conexao);
            if (saldoBanco != 0)
            {
                int novoSaldo = (saldoBanco + cliente.Conta.SaldoPontos);
                if (novoSaldo > PontuacaoMaxima)
                {
                    Notificar("O usuário não pode ter mais de mil pontos");
                    return false;
                }

                if (novoSaldo < 0)
                {
                    Notificar($"Novo Saldo é inválido: {novoSaldo}");
                    return false;
                }

                _contaRepository.AlterarPontosCliente(novoSaldo, idCliente, conexao);
            }
            else
            {
                if (cliente.Conta.SaldoPontos > PontuacaoMaxima)
                {
                    Notificar("O usuário não pode ter mais de mil pontos");
                    return false;
                }

                if (cliente.Conta.SaldoPontos < 0)
                {
                    Notificar($"Novo Saldo é inválido: {cliente.Conta.SaldoPontos}");
                    return false;
                }

                _contaRepository.CriarContaCliente(cliente.Conta.SaldoPontos, idCliente, conexao);
            }

            return true;
        }

        public bool CadastrarCliente(Cliente cliente, string conexao)
        {
            if (!ExecutarValidacao(new ClienteValidation(), cliente)) return false;

            foreach (var endereco in cliente.Enderecos)
            {
                if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return false;
            }

            if (_clienteRepository.ValidarClienteExiste(cliente.CPF, conexao))
            {
                Notificar("Já existe um cliente com esse CPF.");
                return false;
            }

            foreach (var telefone in cliente.Telefones)
            {
                var telExiste = _telefoneRepository.ObterTelefonePorNumero(telefone.Numero, conexao);

                if (telExiste != null)
                {
                    Notificar($"Este número: {telExiste.DDD} {telExiste.Numero} já está cadastrado para outro usuário");
                    return false;
                }
            }

            if (cliente.Conta.SaldoPontos > PontuacaoMaxima)
            {
                Notificar("O usuário não pode ter mais de mil pontos");
                return false;
            }

            int idClienteNovo = _clienteRepository.CadastrarCliente(cliente, conexao);
            if (!_contaRepository.CriarContaCliente(cliente.Conta.SaldoPontos, idClienteNovo, conexao))
            {
                Notificar($"Ocorreu um erro ao cadastrar a conta do cliente: {cliente.Nome} e ID:{idClienteNovo}");
                return false;
            }

            foreach (var endereco in cliente.Enderecos)
            {
                if (!_enderecoRepository.CadastrarEnderecosCliente(endereco, idClienteNovo, conexao))
                {
                    Notificar($"Ocorreu um erro ao cadastrar os endereços do cliente: {cliente.Nome} e ID:{idClienteNovo}");
                    return false;
                } 
            }

            foreach (var telefone in cliente.Telefones)
            {
                if (!_telefoneRepository.CadastrarTelefonesCliente(telefone, idClienteNovo, conexao))
                {
                    Notificar($"Ocorreu um erro ao cadastrar os telefones do cliente: {cliente.Nome} e ID:{idClienteNovo}");
                    return false;
                } 
            }

            return true;
        }

        public bool DeletarCliente(int idCliente, string conexao)
        {
            var clienteBanco = _clienteRepository.ObterDadosCliente(idCliente, conexao);
            if (clienteBanco != null)
            {
                _clienteRepository.DeletarCliente(clienteBanco.Id, conexao);
                return true;
            }
            else
            {
                Notificar("Esse usuário não existe");
                return false;
            }
        }

        public Cliente ObterDadosCliente(int idCliente, string conexao)
        {
            var cliente =  _clienteRepository.ObterDadosCliente(idCliente, conexao);
            if(cliente == null)
            {
                Notificar("Esse cliente não existe");
                return new Cliente();
            }
            return cliente;
        }

        public List<Cliente> ObterDadosTodosClientes(string conexao)
        {
            return _clienteRepository.ObterTodosClientes(conexao);
        }
    }
}
