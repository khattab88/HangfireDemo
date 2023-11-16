
using Hangfire;
using WebAPI.Services;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHangfire(config => 
                config.UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddHangfireServer();

            builder.Services.AddTransient<IServiceManagement, ServiceManagement>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseHangfireDashboard();
            app.MapHangfireDashboard();

            // Recurring Job
            // runs job At second :00 of every minute
            RecurringJob.AddOrUpdate<IServiceManagement>(s => s.SyncData(), "0 * * ? * *");

            app.Run();
        }
    }
}