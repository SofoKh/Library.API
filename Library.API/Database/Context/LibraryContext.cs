using Library.API.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Database.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext()
        {
        }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("Server = (local); Database =Library; Trusted_Connection = True; ");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
