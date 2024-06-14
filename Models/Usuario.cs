using System.ComponentModel.DataAnnotations;

namespace loja.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}