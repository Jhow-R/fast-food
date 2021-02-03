using FastFood.Data.Context;
using FastFood.Data.Repository;
using FastFood.Data.Repository.Interfaces;
using FastFood.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FastFood
{
    // Startup.cs equivalente ao Global.asax.
    // Contém o código de inicialização e configuração.
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Contém a lista de serviços da aplicação
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc(); -- Versão 3.1

            services.AddControllersWithViews()
                // AddRazorRuntimeCompilation(): habilitar refreshing após mudanças na view
                .AddRazorRuntimeCompilation()
                // AddNewtonsoftJson(): erro de serialização no TempData "cannot serialize an object of type"
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // AddTransient: não necessitamos manter estado de nada dentro dele, ou seja, a cada injeção, será uma nova instância resolvida
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();
            services.AddTransient<ILancheRepository, LancheRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();

            // AddScoped: objeto é o mesmo dentro de um request, mas diferente através de diferentes requests
            services.AddScoped(cp => CarrinhoCompras.GetCarrinho(cp));

            // AddSingleton: objeto será o mesmo para todas as requisições
            // HttpContextAccessor: ter acesso a sessão do contexto
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Configuração do uso de sessão
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Recebe o IApplicationBuilder onde pode se configurar o ambiente, página de erros, etc;
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "filtrarPorCategoria",
                    pattern: "Lanches/{action}/{categoria?}",
                    defaults: new { Controller = "Lanches", Action = "List" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            /* -- Versão 3.1
            app.UseMvc(routes =>
             {
                 routes.MapRoute(
                     name: "default",
                     template: "{controller=Home}/{action=Index}/{id?}");
             });*/
        }
    }
}
