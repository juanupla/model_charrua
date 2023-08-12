using Charrua_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Charrua_API.Data
{
    public class ContextBD:DbContext
    {
        public ContextBD(DbContextOptions<ContextBD> options):base(options)
        {

        }

        public DbSet<Usuario> usuarios { get; set; }
    }
}
