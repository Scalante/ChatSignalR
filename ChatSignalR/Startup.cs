using ChatSignalR.Core.Interfaces;
using ChatSignalR.Infrastructure.Context;
using ChatSignalR.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ChatSignalR
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            //Registro del DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDistributedMemoryCache();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.Zero;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
            {
                config.ExpireTimeSpan = TimeSpan.Zero;
                //En false - Permite que la cookie no renueve la hora indicada en el momento de iniciar sesión.
                config.SlidingExpiration = false;
                config.AccessDeniedPath = "/Manage/ErrorAcceso";
            });

            //Necesitamos indicar un provedor de almacenamiento para tempdata
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            //Registro del servicio - SignalR
            services.AddSignalR();

            services.AddControllersWithViews(options => options.EnableEndpointRouting = false).AddSessionStateTempDataProvider();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ADMINISTRADORES", policy => policy.RequireRole("ADMIN"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsRule", rule =>
                {
                    rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                });
            });
        }

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/Chats");
            });
        }

    }
}
