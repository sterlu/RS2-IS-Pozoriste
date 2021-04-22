using System.Collections.Generic;
using MongoDB.Driver;
using Server.Models;

namespace Server.Services
{
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

        public List<Predstava> Get() =>
            _predstave.Find(predstava => true)
                  .ToList();

        public Predstava Get(string id) =>
            _predstave.Find<Predstava>(predstava => predstava.Id == id)
                  .FirstOrDefault();

        public Predstava Create(Predstava predstava)
        {
            _predstave.InsertOne(predstava);
            return predstava;
        }

        public void Update(string id, Predstava newValForPredstava) =>
            _predstave.ReplaceOne(predstava => predstava.Id == id, newValForPredstava);

        public void Remove(Predstava deletePredstava) =>
            _predstave.DeleteOne(predstava => predstava.Id == deletePredstava.Id);

        public void Remove(string id) =>
            _predstave.DeleteOne(predstava => predstava.Id == id);
    }
}