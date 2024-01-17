namespace ride_Core.Entities
{
    public class User : EntityBase
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
    }
}