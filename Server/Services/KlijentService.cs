using System.Collections.Generic;
using MongoDB.Driver;
using Server.Models;

namespace Server.Services
{
    public class KlijentService
    {
        private readonly IMongoCollection<Klijent> _klijenti;

        #region snippet_KlijentServiceConstructor
        public KlijentService(IMyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _klijenti = database.GetCollection<Klijent>(settings.KlijentCollectionName); 
        }
        #endregion

        public List<Klijent> Get() =>
            _klijenti.Find(klijent => true)
                  .ToList();

        public Klijent Get(string id) =>
            _klijenti.Find<Klijent>(klijent => klijent.Id == id)
                  .FirstOrDefault();

        public Klijent Create(Klijent klijent)
        {
            _klijenti.InsertOne(klijent);
            return klijent;
        }

        public void Update(string id, Klijent newValForKlijent) =>
            _klijenti.ReplaceOne(klijent => klijent.Id == id, newValForKlijent);

        public void Remove(Klijent deleteKlijent) =>
            _klijenti.DeleteOne(klijent => klijent.Id == deleteKlijent.Id);

        public void Remove(string id) =>
            _klijenti.DeleteOne(klijent => klijent.Id == id);
    }
    
}