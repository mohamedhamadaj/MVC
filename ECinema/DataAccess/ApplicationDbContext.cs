using Microsoft.EntityFrameworkCore;
using WebApplication2.DataAccess.EntityConfigration;
using ECinema.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ECinema.ViewModels;



namespace ECinema.DataAccess

{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
            : base(options)
        {
        }
        public DbSet<Cinema> Cinemas{ get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieSubimage> MovieSubimages{ get; set; }
        public DbSet<MovieActor>MovieActors{ get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ECinema;Integrated Security=True;Connect Timeout=30;" +
        //        "Encrypt=True;Trust Server Certificate=True;" +
        //        "Application Intent=ReadWrite;Multi Subnet Failover=False");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieImageEntityTypeconfigrations).Assembly);

            base.OnModelCreating(modelBuilder);

        }
        public DbSet<ECinema.ViewModels.RegisterVM> RegisterVM { get; set; } = default!;
        public DbSet<ECinema.ViewModels.LoginVM> LoginVM { get; set; } = default!;
    }
}
