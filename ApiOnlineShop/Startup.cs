using ApiOnlineShop.Repositories;
using ApiOnlineShop.Repositories.Interfaces;
using ApiOnlineShop.Services;
using ApiOnlineShop.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ApiOnlineShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IClientesService, ClientesService>();
            services.AddScoped<IClientesRepository, ClientesRepository>();

            services.AddScoped<IFornecedoresService, FornecedoresService>();
            services.AddScoped<IFornecedoresRepository, FornecedoresRepository>();

            services.AddScoped<IProdutosService, ProdutosService>();
            services.AddScoped<IProdutosRepository, ProdutosRepository>();

            services.AddScoped<IPedidosService, PedidosService>();
            services.AddScoped<IPedidosRepository, PedidosRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiOnlineShop", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiOnlineShop v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
