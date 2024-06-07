using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loja.Data;
using loja.Models;
using Microsoft.EntityFrameworkCore;

namespace loja.Services
{
    public class FornecedorService
    {
        private readonly LojaDbContext _dbContext;

        public FornecedorService(LojaDbContext serviceLocator)
        {
            _dbContext = serviceLocator;
        }

        public async Task<List<Fornecedor>> GetAllFornecedoresAsync()
        {
            return await _dbContext.Fornecedores.ToListAsync();
        }

        public async Task<Fornecedor> GetFornecedorByIdAsync(int id)
        {
            var forncedor = await _dbContext.Fornecedores.FindAsync(id);
            if (forncedor == null)
            {
                throw new KeyNotFoundException($"O Fornecedor com o ID {id} não encontrado.");
            }
            return forncedor;
        }

        public async Task AddFornecedorAsync(Fornecedor forncedor)
        {
            _dbContext.Fornecedores.Add(forncedor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateFornecedorAsync(Fornecedor forncedor)
        {
            _dbContext.Entry(forncedor).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFornecedorAsync(int id)
        {
            var fornecedor = await _dbContext.Fornecedores.FindAsync(id);
            if (fornecedor != null)
            {
                _dbContext.Fornecedores.Remove(fornecedor);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}