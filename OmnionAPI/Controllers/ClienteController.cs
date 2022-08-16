using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Omnion.Business.Interfaces;
using Omnion.Repository.Entities;
using Omnion.Repository.Interfaces;
using OmnionAPI.ViewModel;
using System.Collections.Generic;

namespace OmnionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : MainController
    {
        private readonly IClienteService _clienteService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly ITelefoneRepository _telefoneRepository;
        private readonly IContaRepository _contaRepository;
        private readonly IMapper _mapper;
        public ClienteController(IClienteService clienteService,
                                 IEnderecoRepository enderecoRepository,
                                 ITelefoneRepository telefoneRepository,
                                 IContaRepository contaRepository,
                                 IMapper mapper,
                                 INotificador notificador) : base(notificador)
        {
            _clienteService = clienteService;
            _enderecoRepository = enderecoRepository;
            _telefoneRepository = telefoneRepository;
            _contaRepository = contaRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ClienteViewModel>), 200)]
        public ActionResult<List<ClienteViewModel>> ObterDadosTodosClientes()
        {
            var clientes = _mapper.Map<List<ClienteViewModel>>(_clienteService.ObterDadosTodosClientes(ConnectionString));
            foreach (var cliente in clientes)
            {
                cliente.EnderecosViewModel = _mapper.Map<List<EnderecoViewModel>>(_enderecoRepository.ObterEnderecosCliente(cliente.Id, ConnectionString));
                cliente.TelefonesViewModel = _mapper.Map<List<TelefoneViewModel>>(_telefoneRepository.ObterDadosTelefoneCliente(cliente.Id, ConnectionString));
                cliente.ContaViewModel.SaldoPontos = _contaRepository.VerificarPontuacaoCliente(cliente.Id, ConnectionString);
            }

            return CustomResponse(clientes);
        }

        [HttpGet("{idCliente:int}")]
        public ActionResult<ClienteViewModel> ObterDadosCliente(int idCliente)
        {
            var cliente = _mapper.Map<ClienteViewModel>(_clienteService.ObterDadosCliente(idCliente, ConnectionString));
            cliente.EnderecosViewModel = _mapper.Map<List<EnderecoViewModel>>(_enderecoRepository.ObterEnderecosCliente(cliente.Id, ConnectionString));
            cliente.TelefonesViewModel = _mapper.Map<List<TelefoneViewModel>>(_telefoneRepository.ObterDadosTelefoneCliente(cliente.Id, ConnectionString));
            cliente.ContaViewModel.SaldoPontos = _contaRepository.VerificarPontuacaoCliente(idCliente, ConnectionString);

            return CustomResponse(cliente);
        }

        [HttpPost]
        public ActionResult<ClienteViewModel> CadastrarCliente(ClienteViewModel clienteViewModel)
        {
            var cliente = _mapper.Map<Cliente>(clienteViewModel);
            cliente.Enderecos = _mapper.Map<List<Endereco>>(clienteViewModel.EnderecosViewModel);
            cliente.Telefones = _mapper.Map<List<Telefone>>(clienteViewModel.TelefonesViewModel);
            cliente.Conta.SaldoPontos = clienteViewModel.ContaViewModel.SaldoPontos;

            _clienteService.CadastrarCliente(cliente, ConnectionString);
            return CustomResponse(clienteViewModel);
        }

        [HttpPut("{idCliente:int}")]
        public ActionResult<ClienteViewModel> AtualizarCliente(int idCliente, ClienteViewModel clienteViewModel)
        {
            if (idCliente != clienteViewModel.Id)
            {
                NotificarErro("Os ids não coincidem.");
                return CustomResponse(clienteViewModel);
            }

            var cliente = _mapper.Map<Cliente>(clienteViewModel);
            cliente.Enderecos = _mapper.Map<List<Endereco>>(clienteViewModel.EnderecosViewModel);
            cliente.Telefones = _mapper.Map<List<Telefone>>(clienteViewModel.TelefonesViewModel);
            cliente.Conta.SaldoPontos = clienteViewModel.ContaViewModel.SaldoPontos;

            _clienteService.AtualizarCliente(cliente, idCliente, ConnectionString);
            return CustomResponse("Usuário atualizado!");
        }

        [HttpDelete("{idCliente:int}")]
        public ActionResult<ClienteViewModel> ExcluirCliente(int idCliente)
        {
            var cliente = _clienteService.ObterDadosCliente(idCliente, ConnectionString);
            if (cliente == null) return NotFound();

            _clienteService.DeletarCliente(idCliente, ConnectionString);
            return CustomResponse("Usuário deletado!");
        }

        [HttpPost("{idCliente:int}")]
        public ActionResult<ClienteViewModel> AlterarPontoCliente(int idCliente, ContaViewModel contaViewModel)
        {
            var cliente = _clienteService.ObterDadosCliente(idCliente, ConnectionString);
            if (cliente == null) return NotFound();

            int novoSaldo = _clienteService.AlterarPontosCliente(contaViewModel.SaldoPontos, idCliente, ConnectionString);
            contaViewModel.SaldoPontos = novoSaldo;
            return CustomResponse(contaViewModel);
        }
    }
}
