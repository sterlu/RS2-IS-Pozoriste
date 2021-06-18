using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Services;

namespace Server.Models.Seed
{
    /// Seed za tabelu Korisnik.
    public class KorisnikSeed
    {

        public static void Seed(IApplicationBuilder app, IConfiguration configuration)
        {
            KorisnikService _korisnikService = app.ApplicationServices.GetRequiredService<KorisnikService>();
            
            if (_korisnikService.GetByUsername("admin") == null)
            {
                _korisnikService.Register("admin", configuration.GetValue<string>("InitialAdminPassword"), "admin@pozoriste.com", false, "admin");
            }
        }
    }
}