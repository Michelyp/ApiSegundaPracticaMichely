using ApiSegundaPracticaMichely.Data;
using ApiSegundaPracticaMichely.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSegundaPracticaMichely.Repositories
{
    public class RepositoryCubo
    {
        private CubosContext context;

        public RepositoryCubo(CubosContext context)
        {
            this.context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await this.context.Cubos.ToListAsync();
        }

        public async Task<List<Cubo>> FindCubosMarcaAsync(string marca)
        {
            return await this.context.Cubos
                .Where(x => x.Marca == marca).ToListAsync();
        }
        public async Task<List<string>> GetMarcasAsync()
        {
            var consulta = (from datos in this.context.Cubos
                            select datos.Marca).Distinct();
            return await consulta.ToListAsync();
        }
        private int GetMaxIdUsuario()
        {
            return this.context.Usuarios.Max(x => x.IdUsuario) + 1;
        }
        public async Task InsertUsuarioAsync(string nombre, string email, string pass, string imagen)
        {
            UsuariosCubo user = new UsuariosCubo();
            user.IdUsuario = this.GetMaxIdUsuario();
            user.Nombre = nombre;
            user.Email = email;
            user.Password = pass;
            user.Imagen = imagen;
            await this.context.Usuarios.AddAsync(user);
            await this.context.SaveChangesAsync();
        }
        public async Task<UsuariosCubo>
         LogInEmpleadoAsync(string email, string pass)
        {
            return await this.context.Usuarios
                .Where(x => x.Email == email
                && x.Password == pass)
                .FirstOrDefaultAsync();
        }
        public async Task<List<ComprasCubo>> FindPedidosUsuarioAsync(int idusuario)
        {
            return await this.context.Compras
                .Where(x => x.IdUsuario == idusuario).ToListAsync();
        }

    }
}
