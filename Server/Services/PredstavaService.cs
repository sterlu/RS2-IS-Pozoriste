using System.Collections.Generic;
using MongoDB.Driver;
using Server.Models;

namespace Server.Services
{
    /// Servis koji omogućava čitanje, pisanje, menjanje i brisanje podataka iz tabele Predstava.
    public class PredstavaService
    {
        private readonly IMongoCollection<Predstava> _predstave;

        #region snippet_predstavaServiceConstructor
        public PredstavaService(IMyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _predstave = database.GetCollection<Predstava>(settings.PredstavaCollectionName); 
        }
        #endregion

        /// Vraća sve predstave iz tabele.
        public List<Predstava> Get() =>
            _predstave.Find(predstava => true)
                  .ToList();

        /// Vraća odredjenu predstavu.
        public Predstava Get(string id) =>
            _predstave.Find<Predstava>(predstava => predstava.Id == id)
                  .FirstOrDefault();
        
        /// Upisuje novu predstavu u bazu.
        public Predstava Create(Predstava predstava)
        {
            _predstave.InsertOne(predstava);
            return predstava;
        }

        /// Menja postojeću vrednost u bazi.
        /// @param id - id vrednosti koja se menja.
        /// @param newValForPredstava - nova vrednost.
        public void Update(string id, Predstava newValForPredstava) =>
            _predstave.ReplaceOne(predstava => predstava.Id == id, newValForPredstava);

        /// Briše predstavu iz baze.
        public void Remove(Predstava deletePredstava) =>
            _predstave.DeleteOne(predstava => predstava.Id == deletePredstava.Id);

         /// Briše predstavu iz baze na osnovu vrednosti atributa id.
        public void Remove(string id) =>
            _predstave.DeleteOne(predstava => predstava.Id == id);
    }
}