using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PaymentService.Models;
using PaymentService.Services;

namespace PaymentService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MyDatabaseSettings>(Configuration.GetSection(nameof(MyDatabaseSettings)));
            services.AddSingleton<IMyDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MyDatabaseSettings>>().Value);

            services.Configure<DomainSettings>(Configuration.GetSection(nameof(DomainSettings)));
            services.AddSingleton<DomainSettings>(sp => sp.GetRequiredService<IOptions<DomainSettings>>().Value);

            services.AddSingleton<PlacanjeService>();
            services.AddSingleton<KartaService>();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "PaymentService", Version = "v1"});
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentService v1"));
            }
            
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            Stripe.StripeConfiguration.ApiKey = Configuration.GetValue<string>("StripeSecretKey");
        }
    }
}