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
using Server.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Proxy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using WebPush;
using Server.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.Models.Seed;

namespace Server
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

            services.Configure<MyDatabaseSettings>(Configuration.GetSection(nameof(MyDatabaseSettings)));
            services.AddSingleton<IMyDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MyDatabaseSettings>>().Value);
            
            services.Configure<DomainSettings>(Configuration.GetSection(nameof(DomainSettings)));
            services.AddSingleton<DomainSettings>(sp => sp.GetRequiredService<IOptions<DomainSettings>>().Value);

            services.AddSingleton<KartaService>();
            services.AddSingleton<KorisnikService>();
            services.AddSingleton<PredstavaService>();
            services.AddSingleton<SalaService>();
            services.AddSingleton<IzvodjenjePredstaveService>();
            services.AddSingleton<PushPretplataService>();
            services.AddSingleton<PlacanjeService>();
            services.AddSingleton<MailingService>();

            services.AddProxies();

            services.AddControllers()
                    .AddNewtonsoftJson(options => options.UseMemberCasing());

            services.AddCors();

            services.AddScoped<ITokenService, TokenService>();

            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
            
            var vapidDetails = new VapidDetails(
                Configuration.GetValue<string>("VapidDetails:Subject"),
                Configuration.GetValue<string>("VapidDetails:PublicKey"),
                Configuration.GetValue<string>("VapidDetails:PrivateKey"));
            services.AddTransient(c => vapidDetails);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseSpaStaticFiles();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200"); // Proxy-uj zahteve ka Angular dev serveru pokrenutom eksterno
                    // spa.UseAngularCliServer(npmScript: "start"); // Pokreni Angular dev server u okviru dotnet procesa
                }
            });

            Stripe.StripeConfiguration.ApiKey = Configuration.GetValue<string>("StripeSecretKey");
            
            SalaSeed.Seed(app, Configuration);
            KorisnikSeed.Seed(app, Configuration);
        }
    }
}
