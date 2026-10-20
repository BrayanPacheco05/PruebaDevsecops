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
        
        public DbSet<TelefonoModel> Telefonos { get; set; }
        public DbSet<ContactoTelefonoModel> ContactosTelefonos { get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactoModel>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<ContactoTelefonoModel>()
                .HasKey(ct => new { ct.ContactoId, ct.TelefonoId });

            modelBuilder.Entity<ContactoTelefonoModel>()
                .HasOne(ct => ct.Contacto)
                .WithMany(c => c.ContactosTelefonos)
                .HasForeignKey(ct => ct.ContactoId);

            modelBuilder.Entity<ContactoTelefonoModel>()
                .HasOne(ct => ct.Telefono)
                .WithMany(t => t.ContactosTelefonos)
                .HasForeignKey(ct => ct.TelefonoId);
        }
    }
}