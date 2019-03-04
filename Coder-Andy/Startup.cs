using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoderAndy.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoderAndy
{
    public class Startup
    {
        public Startup(IConfiguration a_configuration)
        {
            Configuration = a_configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection a_services)
        {
            // Set-up Database connection
            a_services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(
                    Configuration.GetConnectionString("DefaultMYSQLConnection")));

            // Set-up Identity
            a_services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            a_services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder a_app, IHostingEnvironment a_env)
        {
            if (a_env.IsDevelopment())
            {
                a_app.UseDeveloperExceptionPage();
                a_app.UseDatabaseErrorPage();
            }
            else
            {
                a_app.UseExceptionHandler("/Portfolio/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                a_app.UseHsts();
            }

            a_app.UseHttpsRedirection();
            a_app.UseStaticFiles();

            a_app.UseAuthentication();

            a_app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Portfolio}/{action=Index}/{id?}");
            });
        }
    }
}
