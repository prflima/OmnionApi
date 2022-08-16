namespace Omnion.Repository.Entities
{
    public class Endereco : Entity
    {
        public int ClienteId { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string CEP { get; set; }
    }
}
