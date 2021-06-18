using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using PaymentService.Models;

namespace PaymentService.Services
{
    /// Servis koji omogućava čitanje, pisanje, menjanje i brisanje podataka iz tabele Karta.
    public class KartaService
    {
        private readonly IMongoCollection<Karta> _karte;

        #region snippet_KartaServiceConstructor

        public KartaService(IMyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _karte = database.GetCollection<Karta>(settings.KartaCollectionName); 
        }
        #endregion

        /// Vraća sve karte koje se nalaze u bazi.
        public List<Karta> Get() =>
            _karte.Find(karta => true)
                  .ToList();

        /// Vraća odredjenu kartu.
        public Karta Get(string id) =>
            _karte.Find<Karta>(karta => karta.Id == id)
                  .FirstOrDefault();

        /// Vraća sve karte jedne rezervacije.
        public List<Karta> GetAllForReservation(string idRezervacije) =>
            _karte.Find<Karta>(karta => karta.IdRezervacije == idRezervacije)
                  .ToList();

        /// Vraća jednu kartu odredjene rezervacije.
        public Karta GetByRezervacijaId(string idRezervacije) =>
            _karte.Find<Karta>(karta => karta.IdRezervacije == idRezervacije)
                  .FirstOrDefault();

        /// Upisuje novu kartu u bazu, ukoliko ima slobodnih karata.
        public Karta Create(Karta karta)
        { 
            _karte.InsertOne(karta);
            return karta;
        }

        /// Menja postojeću vrednost u bazi.
        /// @param id - id vrednosti koja se menja.
        /// @param newValForKarta - nova vrednost.
        public void Update(string id, Karta newValForKarta) =>
            _karte.ReplaceOne(karta => karta.Id == id, newValForKarta);

        /// Briše kartu iz baze.
        public void Remove(Karta deleteKarta) =>
            _karte.DeleteOne(karta => karta.Id == deleteKarta.Id);

        /// Briše kartu iz baze na osnovu vrednosti atributa id.
        public void Remove(string id) =>
            _karte.DeleteOne(karta => karta.Id == id);
    }
}
