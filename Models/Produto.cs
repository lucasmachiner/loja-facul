using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace loja.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public double Preco { get; set; }
        public Fornecedor? Fornecedor { get; set; }
    }
}