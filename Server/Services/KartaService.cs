using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Server.Models;

namespace Server.Services
{
    public class KartaService
    {
        private readonly IMongoCollection<Karta> _karte;

        #region snippet_KartaServiceConstructor

        private IzvodjenjePredstaveService _izvodjenjeService;
        public KartaService(IMyDatabaseSettings settings, IzvodjenjePredstaveService izvodjenjeService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _karte = database.GetCollection<Karta>(settings.KartaCollectionName); 
            _izvodjenjeService = izvodjenjeService;
        }
        #endregion

        public List<Karta> Get() =>
            _karte.Find(karta => true)
                  .ToList();

        public Karta Get(string id) =>
            _karte.Find<Karta>(karta => karta.Id == id)
                  .FirstOrDefault();

        public List<Karta> GetAllForReservation(string idRezervacije) =>
            _karte.Find<Karta>(karta => karta.IdRezervacije == idRezervacije)
                  .ToList();

        public Karta GetByRezervacijaId(string idRezervacije) =>
            _karte.Find<Karta>(karta => karta.IdRezervacije == idRezervacije)
                  .FirstOrDefault();

        public Karta Create(Karta karta)
        { 
            string idIzvodjenja = karta.IdIzvodjenja;
            var izvodjenje = _izvodjenjeService.GetByIdIzvodjenja(idIzvodjenja);
            if(izvodjenje.BrojSlobodnihKarata > 0)
            {
                _karte.InsertOne(karta);
                izvodjenje.BrojSlobodnihKarata-=1;
                _izvodjenjeService.Update(izvodjenje.Id, izvodjenje);
                return karta;
            }
            return null;
        }

        public void Update(string id, Karta newValForKarta) =>
            _karte.ReplaceOne(karta => karta.Id == id, newValForKarta);

        public void Remove(Karta deleteKarta) =>
            _karte.DeleteOne(karta => karta.Id == deleteKarta.Id);

        public void Remove(string id) =>
            _karte.DeleteOne(karta => karta.Id == id);
    }
}
