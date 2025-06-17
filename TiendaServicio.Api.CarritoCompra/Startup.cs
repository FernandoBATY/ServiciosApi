using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicio.Api.CarritoCompra
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Método para registrar servicios en el contenedor de dependencias
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuración de controladores
            services.AddControllers();

            // Configuración de DbContext con MySQL
            services.AddDbContext<CarritoContexto>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("MySqlConnection"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("MySqlConnection"))
                )
            );

            // Configuración de MediatR
            services.AddMediatR(typeof(TiendaServicios.Api.CarritoCompra.Aplicacion.Nuevo).Assembly);

            // Configuración de Swagger
            services.AddSwaggerGen();

            // Configuración de CORS (puedes ajustar el origen para producción)
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        // Método para configurar la tubería HTTP
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            // Habilita CORS antes de Authorization y Endpoints
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
               