using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ASPProject.Repositories;

namespace ASPProject
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup(IConfiguration configuration )
        {
            this.configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRepository, AnimalRepostory>();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AnimalContext>(options => options.UseSqlServer(connectionString));
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AnimalContext cntx)
        {
            cntx.Database.EnsureDeleted();
            cntx.Database.EnsureCreated();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else if (env.IsStaging() || env.IsProduction())
            {
                app.UseExceptionHandler("/Error/Index");
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Default", "{controller=Animal}/{action=Index}/{id?}");
            });
        }
    }
}
