using System.Collections.Generic;
using MongoDB.Driver;
using Server.Models;

namespace Server.Services
{
    public class SalaService
    {
        private readonly IMongoCollection<Sala> _sale;

        #region snippet_SalaServiceConstructor
         public SalaService(IMyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _sale = database.GetCollection<Sala>(settings.SalaCollectionName); 
        }
        #endregion

        public List<Sala> Get() =>
            _sale.Find(sala => true)
                  .ToList();

        public Sala Get(string id) =>
            _sale.Find<Sala>(sala => sala.Id == id)
                  .FirstOrDefault();
        
        public Sala GetByBrojSale(int broj) =>
            _sale.Find<Sala>(sala => sala.BrojSale == broj)
                  .FirstOrDefault();

        public Sala Create(Sala sala)
        {
            _sale.InsertOne(sala);
            return sala;
        }

        public void Update(string id, Sala newValForSala) =>
            _sale.ReplaceOne(sala => sala.Id == id, newValForSala);

        public void Remove(Sala deleteSala) =>
            _sale.DeleteOne(sala => sala.Id == deleteSala.Id);

        public void Remove(string id) =>
            _sale.DeleteOne(sala => sala.Id == id);
    }
}