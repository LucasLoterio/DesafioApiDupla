using DesafioApi.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DesafioApi.Data
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
