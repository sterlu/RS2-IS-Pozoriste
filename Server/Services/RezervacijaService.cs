using System.Collections.Generic;
using MongoDB.Driver;
using Server.Models;

namespace Server.Services
{
    public class RezervacijaService
    {
        private readonly IMongoCollection<Rezervacija> _rezervacije;

        #region snippet_RezervacijaServiceConstructor
        public RezervacijaService(IMyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _rezervacije = database.GetCollection<Rezervacija>(settings.RezervacijaCollectionName); 
        }
        #endregion

        public List<Rezervacija> Get() =>
            _rezervacije.Find(rezervacija => true)
                  .ToList();

        public Rezervacija Get(string id) =>
            _rezervacije.Find<Rezervacija>(rezervacija => rezervacija.Id == id)
                  .FirstOrDefault();

        public Rezervacija Create(Rezervacija rezervacija)
        {
            _rezervacije.InsertOne(rezervacija);
            return rezervacija;
        }

        public void Update(string id, Rezervacija newValForRezervacija) =>
            _rezervacije.ReplaceOne(rezervacija => rezervacija.Id == id, newValForRezervacija);

        public void Remove(Rezervacija deleteRezervacija) =>
            _rezervacije.DeleteOne(rezervacija => rezervacija.Id == deleteRezervacija.Id);

        public void Remove(string id) =>
            _rezervacije.DeleteOne(rezervacija => rezervacija.Id == id);
    }
}