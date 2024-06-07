using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loja.Models;
using loja.Data;
using Microsoft.EntityFrameworkCore;


namespace loja.Services
{
    public class ProductService
    {
        private readonly LojaDbContext _dbContext;

        public ProductService(LojaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Metodo para consultar todos os produtos
        public async Task<List<Produto>> GetAllProductsAsync()
        {
            return await _dbContext.Produtos.ToListAsync();
        }

        // Métodd para consultar um produto a partir do seu Id
        public async Task<Produto> GetProductByIdAsync(int id)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);
            if (produto == null)
            {
                throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");
            }
            return produto;
        }
        // Método para gravar um novo produto
        public async Task AddProductAsync(Produto produto)
        {
            _dbContext.Produtos.Add(produto);
            await _dbContext.SaveChangesAsync();
        }
        // Método para atualizar os dados de um produto
        public async Task UpdateProductAsync(Produto produto)
        {
            _dbContext.Entry(produto).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        // Método para excluir um produto
        public async Task DeleteProductAsync(int id)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);
            if (produto != null)
            {
                _dbContext.Produtos.Remove(produto);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}