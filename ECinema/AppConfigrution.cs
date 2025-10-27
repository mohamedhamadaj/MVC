using Microsoft.EntityFrameworkCore;

namespace ECinema
{
    public static class AppConfigrution
    {
        public static void RegisterConfig(this IServiceCollection services, string connection)
        {
            services.AddDbContext<ApplicationDbContext>(Option =>
            {
                // Option.UseSqlServer(builder.Configuration["ConnectionStrings : DefaultConnection"]);
                Option.UseSqlServer(connection);
            });

            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IRepository<Cinema>, Repository<Cinema>>();
            services.AddScoped<IRepository<Movie>, Repository<Movie>>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IRepository<MovieSubimage>, Repository<MovieSubimage>>();
        }
    }
}
