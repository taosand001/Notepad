using Notepad.Api.Extension;
using Notepad.Business.Extension;
using Notepad.Database.Extension;
namespace Notepad.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwagger();
            builder.Services.AddBusinessLogic();
            builder.Services.AddJwtExtension(builder.Configuration);
            builder.Services.AddDatabase(builder.Configuration.GetConnectionString("DefaultConnection"));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
