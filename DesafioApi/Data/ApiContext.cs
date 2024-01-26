using DesafioApi.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DesafioApi.Data
{
    public class ApiContext :DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<DesafioApi.Entidades.Produto> Produtos { get; set; } = default!;
        public DbSet<User> Users { get; set; }
    }
}
