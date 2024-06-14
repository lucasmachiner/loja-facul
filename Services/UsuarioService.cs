using loja.Data;
using loja.Models;
using Microsoft.EntityFrameworkCore;

namespace loja.Services
{
    public class UsuarioService
    {
        private readonly LojaDbContext _dbContext;
        public UsuarioService(LojaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Usuario>> GetAllUsuarios()
        {
            return await _dbContext.Usuarios.ToListAsync();
        }
        public async Task<Usuario> GetUsuarioByAsync(int id)
        {
            var usuario = await _dbContext.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException($"Usuario com o ID {id} não encontrado");
            }
            return usuario;
        }
        public async Task AddUsuarioAsync(Usuario usuario)
        {
            _dbContext.Usuarios.Add(usuario);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            _dbContext.Entry(usuario).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteUsuarioAsync(int id)
        {
            var usuario = await _dbContext.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _dbContext.Usuarios.Remove(usuario);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}