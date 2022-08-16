namespace Omnion.Repository.Entities
{
    public class Telefone : Entity
    {
        public int ClienteId { get; set; }
        public int DDD { get; set; }
        public string Numero { get; set; }
    }
}
