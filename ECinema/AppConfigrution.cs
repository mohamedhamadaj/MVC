using ECinema.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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

            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 8;
                option.Password.RequireNonAlphanumeric = false;
                option.User.RequireUniqueEmail = true;
                option.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IRepository<Cinema>, Repository<Cinema>>();
            services.AddScoped<IRepository<Movie>, Repository<Movie>>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IRepository<MovieSubimage>, Repository<MovieSubimage>>();
        }
    }
}
