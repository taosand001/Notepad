using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notepad.Database.Data;
using Notepad.Database.Interfaces;
using Notepad.Database.Repository;

namespace Notepad.Database.Extension
{
    public static class DbExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<NotepadContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();

            return services;
        }
    }
}
