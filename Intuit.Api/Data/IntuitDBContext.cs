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

                // Datos seed para pruebas
                e.HasData(
                    new Client { ClientId = 1, FirstName = "Ana", LastName = "García", BirthDate = new(1990, 5, 12), Cuit = "27-12345678-5", Address = "Calle Falsa 123", Mobile = "+5491122334455", Email = "ana@example.com" },
                    new Client { ClientId = 2, FirstName = "Luis", LastName = "Pérez", BirthDate = new(1985, 4, 8), Cuit = "20-11111111-9", Address = "Av. Siempreviva 742", Mobile = "+5491144455566", Email = "luis@example.com" },

                    new Client { ClientId = 3, FirstName = "Carlos", LastName = "López", BirthDate = new(1988, 7, 1), Cuit = "20-22222222-3", Address = "Belgrano 101", Mobile = "+5491100000003", Email = "carlos@example.com" },
                    new Client { ClientId = 4, FirstName = "María", LastName = "Fernández", BirthDate = new(1992, 3, 22), Cuit = "27-23456789-2", Address = "Mitre 220", Mobile = "+5491100000004", Email = "maria@example.com" },
                    new Client { ClientId = 5, FirstName = "Julián", LastName = "Martínez", BirthDate = new(1991, 9, 14), Cuit = "20-33333333-6", Address = "Rivadavia 330", Mobile = "+5491100000005", Email = "julian@example.com" },
                    new Client { ClientId = 6, FirstName = "Sofía", LastName = "Rodríguez", BirthDate = new(1993, 1, 5), Cuit = "27-34567890-1", Address = "Sarmiento 450", Mobile = "+5491100000006", Email = "sofia@example.com" },
                    new Client { ClientId = 7, FirstName = "Pedro", LastName = "Sánchez", BirthDate = new(1987, 11, 2), Cuit = "20-44444444-8", Address = "San Martín 512", Mobile = "+5491100000007", Email = "pedro@example.com" },
                    new Client { ClientId = 8, FirstName = "Lucía", LastName = "Gómez", BirthDate = new(1995, 6, 30), Cuit = "27-45678901-0", Address = "Alsina 600", Mobile = "+5491100000008", Email = "lucia@example.com" },
                    new Client { ClientId = 9, FirstName = "Diego", LastName = "Torres", BirthDate = new(1989, 10, 18), Cuit = "20-55555555-5", Address = "Urquiza 710", Mobile = "+5491100000009", Email = "diego@example.com" },
                    new Client { ClientId = 10, FirstName = "Valentina", LastName = "Díaz", BirthDate = new(1994, 2, 27), Cuit = "27-56789012-8", Address = "Moreno 820", Mobile = "+5491100000010", Email = "valentina@example.com" },
                    new Client { ClientId = 11, FirstName = "Matías", LastName = "Romero", BirthDate = new(1986, 12, 9), Cuit = "20-66666666-1", Address = "Lavalle 930", Mobile = "+5491100000011", Email = "matias@example.com" },
                    new Client { ClientId = 12, FirstName = "Camila", LastName = "Ruiz", BirthDate = new(1990, 8, 3), Cuit = "27-67890123-6", Address = "Catamarca 1040", Mobile = "+5491100000012", Email = "camila@example.com" },
                    new Client { ClientId = 13, FirstName = "Fernando", LastName = "Gutiérrez", BirthDate = new(1983, 4, 16), Cuit = "20-77777777-7", Address = "Córdoba 1150", Mobile = "+5491100000013", Email = "fernando@example.com" },
                    new Client { ClientId = 14, FirstName = "Paula", LastName = "Silva", BirthDate = new(1991, 1, 25), Cuit = "27-78901234-4", Address = "Tucumán 1260", Mobile = "+5491100000014", Email = "paula@example.com" },
                    new Client { ClientId = 15, FirstName = "Agustín", LastName = "Molina", BirthDate = new(1988, 5, 7), Cuit = "20-88888888-9", Address = "Salta 1370", Mobile = "+5491100000015", Email = "agustin@example.com" },
                    new Client { ClientId = 16, FirstName = "Julieta", LastName = "Castro", BirthDate = new(1996, 7, 19), Cuit = "27-80123456-7", Address = "Mendoza 1480", Mobile = "+5491100000016", Email = "julieta@example.com" },
                    new Client { ClientId = 17, FirstName = "Ramiro", LastName = "Navarro", BirthDate = new(1984, 3, 11), Cuit = "20-99999999-0", Address = "Entre Ríos 1590", Mobile = "+5491100000017", Email = "ramiro@example.com" },
                    new Client { ClientId = 18, FirstName = "Bianca", LastName = "Rojas", BirthDate = new(1993, 9, 2), Cuit = "27-81234567-5", Address = "San Juan 1700", Mobile = "+5491100000018", Email = "bianca@example.com" },
                    new Client { ClientId = 19, FirstName = "Nicolás", LastName = "Vega", BirthDate = new(1987, 2, 13), Cuit = "20-12121212-3", Address = "Santiago 1810", Mobile = "+5491100000019", Email = "nicolas@example.com" },
                    new Client { ClientId = 20, FirstName = "Elena", LastName = "Acosta", BirthDate = new(1992, 11, 21), Cuit = "27-82345678-3", Address = "Jujuy 1920", Mobile = "+5491100000020", Email = "elena@example.com" }
                );
            });
        }
    }
}
