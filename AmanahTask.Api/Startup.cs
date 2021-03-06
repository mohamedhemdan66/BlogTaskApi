using AmanahTask.Core;
using AmanahTask.Core.Interfaces;
using AmanahTask.EF;
using AmanahTask.EF.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmanahTask.Api
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
            services.AddDbContextPool<AmanahDbContext>(o => o.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection"),
                  a => a.MigrationsAssembly(typeof(AmanahDbContext).Assembly.FullName)));



            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<ILogService, LogService>();

            services.AddAutoMapper(typeof(Startup));

            services.AddCors(
                a => a.AddPolicy("CORS",
                p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
                );

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmanahTask.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AmanahTask.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseCors("CORS");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
