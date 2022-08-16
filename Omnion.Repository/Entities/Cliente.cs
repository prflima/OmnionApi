using System;
using System.Collections.Generic;

namespace Omnion.Repository.Entities
{

    public class Cliente : Entity
    {
        public Cliente()
        {
            Enderecos = new List<Endereco>();
            Telefones = new List<Telefone>();
            Conta = new Conta();
        }

        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }

        public List<Endereco> Enderecos { get; set; }
        public List<Telefone> Telefones { get; set; }
        public Conta Conta { get; set; }
    }
}
