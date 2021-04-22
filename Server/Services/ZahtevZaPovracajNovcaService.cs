using System.Collections.Generic;
using MongoDB.Driver;
using Server.Models;

namespace Server.Services
{
    public class ZahtevZaPovracajNovcaService
    {
        private readonly IMongoCollection<ZahtevZaPovracajNovca> _zahtevi;

        #region snippet_ZahtevZaPovracajNovcaServiceConstructor
        public ZahtevZaPovracajNovcaService(IMyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _zahtevi = database.GetCollection<ZahtevZaPovracajNovca>(settings.ZahtevZaPovracajNovcaCollectionName); 
        }
        #endregion

        public List<ZahtevZaPovracajNovca> Get() =>
            _zahtevi.Find(zahtev => true)
                  .ToList();

        public ZahtevZaPovracajNovca Get(string id) =>
            _zahtevi.Find<ZahtevZaPovracajNovca>(zahtev => zahtev.Id == id)
                  .FirstOrDefault();

        public ZahtevZaPovracajNovca Create(ZahtevZaPovracajNovca zahtev)
        {
            _zahtevi.InsertOne(zahtev);
            return zahtev;
        }

        public void Update(string id, ZahtevZaPovracajNovca newValForZahtev) =>
            _zahtevi.ReplaceOne(zahtev => zahtev.Id == id, newValForZahtev);

        public void Remove(ZahtevZaPovracajNovca deleteZahtev) =>
            _zahtevi.DeleteOne(zahtev => zahtev.Id == deleteZahtev.Id);

        public void Remove(string id) =>
            _zahtevi.DeleteOne(zahtev => zahtev.Id == id);
    }
}