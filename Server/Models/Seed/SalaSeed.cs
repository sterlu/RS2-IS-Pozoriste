using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Services;

namespace Server.Models.Seed
{
    public class SalaSeed
    {
        
        public static async void Seed(IApplicationBuilder app, IConfiguration configuration)
        {
            SalaService _salaService = app.ApplicationServices.GetRequiredService<SalaService>();
            
            if (_salaService.GetByBrojSale(1) == null) {
                _salaService.Create(new Sala(1, 120, "Velika sala"));
            }
            if (_salaService.GetByBrojSale(2) == null) {
                _salaService.Create(new Sala(2, 70, "Mala sala A"));
            }
            if (_salaService.GetByBrojSale(3) == null) {
                _salaService.Create(new Sala(3, 60, "Mala sala B"));
            }
        }
    }
}