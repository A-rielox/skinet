using API.Extensions;
using API.Helpers;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddControllers();

            services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });

            services.AddApplicationServices(); // p' lo de mi metodo de extension

            services.AddSwaggerDocumentation();

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // p' q pase x mi middleware que checa los errores de exception como el de
            // " System.NullReferenceException ", los cacha y manda el error formateado
            // tiene q ser aca arriba
            app.UseMiddleware<ExceptionMiddleware>();

            //if (env.IsDevelopment())
            //{
                //app.UseDeveloperExceptionPage(); lo quito p'q corra bien app.UseMiddleware<ExceptionMiddleware>();
            //}

            // para redireccionar los errores con mi controller de errores, p' el caso en que no existe la ruta
            // como https://localhost:5001/api/asdf
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles(); // para servir static assets ( lo de la carpeta wwwroot )

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseSwaggerDocumentation();  // p' lo de mi metodo de extension

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
