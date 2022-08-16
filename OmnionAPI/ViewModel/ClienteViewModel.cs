using System;
using System.Collections.Generic;

namespace OmnionAPI.ViewModel
{
    public class ClienteViewModel
    {
        public ClienteViewModel()
        {
            EnderecosViewModel = new List<EnderecoViewModel>();
            TelefonesViewModel = new List<TelefoneViewModel>();
            ContaViewModel = new ContaViewModel();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }

        public List<EnderecoViewModel> EnderecosViewModel{ get; set; }
        public List<TelefoneViewModel> TelefonesViewModel { get; set; }
        public ContaViewModel ContaViewModel { get; set; }
    }
}
