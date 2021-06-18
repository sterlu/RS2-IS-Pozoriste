using System.Collections.Generic;
using MongoDB.Driver;
using Server.Models;

namespace Server.Services
{
    /// Servis koji omogućava čitanje, pisanje, menjanje i brisanje podataka iz tabele Sala.
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

        /// Vraća sve sale iz tabele. 
        public List<Sala> Get() =>
            _sale.Find(sala => true)
                  .ToList();

        /// Vraća odredjenu salu.
        public Sala Get(string id) =>
            _sale.Find<Sala>(sala => sala.Id == id)
                  .FirstOrDefault();
        
        /// Vraća salu na osnovu njenog broja.
        public Sala GetByBrojSale(int broj) =>
            _sale.Find<Sala>(sala => sala.BrojSale == broj)
                  .FirstOrDefault();

        /// Upisuje novu salu u bazu.
        public Sala Create(Sala sala)
        {
            _sale.InsertOne(sala);
            return sala;
        }

        /// Menja postojeću vrednost u bazi.
        /// @param id - id vrednosti koja se menja.
        /// @param newValForSala - nova vrednost.
        public void Update(string id, Sala newValForSala) =>
            _sale.ReplaceOne(sala => sala.Id == id, newValForSala);

        /// Briše salu iz baze.
        public void Remove(Sala deleteSala) =>
            _sale.DeleteOne(sala => sala.Id == deleteSala.Id);

        /// Briše salu iz baze na osnovu vrednosti atributa id.
        public void Remove(string id) =>
            _sale.DeleteOne(sala => sala.Id == id);
    }
}