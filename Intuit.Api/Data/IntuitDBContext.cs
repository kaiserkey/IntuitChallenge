using Intuit.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Intuit.Api.Data
{
    /// <summary>
    /// Database context for Intuit API.
    /// </summary>
    public class IntuitDBContext(DbContextOptions<IntuitDBContext> options) : DbContext(options)
    {
        public DbSet<Client> Client { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Client>(e =>
            {
                e.ToTable("Client");
                e.HasKey(x => x.ClientId);
                e.Property(x => x.ClientId).ValueGeneratedOnAdd();

                e.Property(x => x.FirstName).HasColumnName("FirstName").HasMaxLength(100).IsRequired();
                e.Property(x => x.LastName).HasColumnName("LastName").HasMaxLength(100).IsRequired();
                e.Property(x => x.BirthDate).HasColumnName("BirthDate").IsRequired();
                e.Property(x => x.Cuit).HasColumnName("Cuit").HasMaxLength(20).IsRequired();
                e.Property(x => x.Address).HasColumnName("Address").HasMaxLength(200);
                e.Property(x => x.Mobile).HasColumnName("Mobile").HasMaxLength(30).IsRequired();
                e.Property(x => x.Email).HasColumnName("Email").HasMaxLength(200).IsRequired();

                e.HasIndex(x => x.Cuit).IsUnique();
                e.HasIndex(x => x.Email).IsUnique();

                // Datos seed
                e.HasData(
                    new Client { ClientId = 1, FirstName = "Ana", LastName = "García", BirthDate = new(1990, 5, 12), Cuit = "27-12345678-5", Address = "Calle Falsa 123", Mobile = "+5491122334455", Email = "ana@example.com" },
                    new Client { ClientId = 2, FirstName = "Luis", LastName = "Pérez", BirthDate = new(1985, 4, 8), Cuit = "20-11111111-9", Address = "Av. Siempreviva 742", Mobile = "+5491144455566", Email = "luis@example.com" }
                );
            });
        }
    }
}
