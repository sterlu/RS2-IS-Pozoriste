using System.Collections.Generic;
using MongoDB.Driver;
using Server.Models;

namespace Server.Services
{
    public class ZaposlenService
    {
        private readonly IMongoCollection<Zaposlen> _zaposleni;

        #region snippet_ZaposlenServiceConstructor
        public ZaposlenService(IMyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _zaposleni = database.GetCollection<Zaposlen>(settings.ZaposlenCollectionName); 
        }
        #endregion

        public List<Zaposlen> Get() =>
            _zaposleni.Find(zaposlen => true)
                  .ToList();

        public Zaposlen Get(string id) =>
            _zaposleni.Find<Zaposlen>(zaposlen => zaposlen.Id == id)
                  .FirstOrDefault();

        public Zaposlen Create(Zaposlen zaposlen)
        {
            _zaposleni.InsertOne(zaposlen);
            return zaposlen;
        }

        public void Update(string id, Zaposlen newValForZaposlen) =>
            _zaposleni.ReplaceOne(zaposlen => zaposlen.Id == id, newValForZaposlen);

        public void Remove(Zaposlen deleteZaposlen) =>
            _zaposleni.DeleteOne(zaposlen => zaposlen.Id == deleteZaposlen.Id);

        public void Remove(string id) =>
            _zaposleni.DeleteOne(zaposlen => zaposlen.Id == id);
    }
    
}