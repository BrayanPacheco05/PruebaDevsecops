// Data/AppDbContext.cs

using Microsoft.EntityFrameworkCore;
using CSharpConctactApi.Models;


namespace CSharpConctactApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ContactoModel> Contacto { get; set; }
    }
}