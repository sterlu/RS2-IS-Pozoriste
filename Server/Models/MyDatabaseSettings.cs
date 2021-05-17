using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    // Klasa u kojoj cemo cuvati vrednosti iz appsettings.json vezane za bazu
    // Primena - u ConfigureServices za mapiranje vrednosti iz .json fajla
    public class MyDatabaseSettings : IMyDatabaseSettings
    {
        public string KartaCollectionName { get; set; }
        public string KlijentCollectionName { get; set; }
        public string PredstavaCollectionName { get; set; }
        public string RezervacijaCollectionName { get; set; }
        public string SalaCollectionName { get; set; }
        public string ZahtevZaPovracajNovcaCollectionName { get; set; }
        public string ZaposlenCollectionName { get; set; }
        public string PushPretplateCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMyDatabaseSettings
    {
        string KartaCollectionName { get; set; }
        string KlijentCollectionName { get; set; }
        string PredstavaCollectionName { get; set; }
        string RezervacijaCollectionName { get; set; }
        string SalaCollectionName { get; set; }
        string ZahtevZaPovracajNovcaCollectionName { get; set; }
        string ZaposlenCollectionName { get; set; }
        string PushPretplateCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
