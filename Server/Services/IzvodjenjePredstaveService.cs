using Server.Models;
using System.Collections.Generic;
using MongoDB.Driver;
using System;

namespace Server.Services
{
    /// Servis koji omogućava čitanje, pisanje, menjanje i brisanje podataka iz tabele IzvodjenjePredstave.
    public class IzvodjenjePredstaveService
    {
        private readonly IMongoCollection<IzvodjenjePredstave> _izvodjenja;

        #region snippet_IzvodnjenjePredstaveServiceConstructor
         public IzvodjenjePredstaveService(IMyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _izvodjenja = database.GetCollection<IzvodjenjePredstave>(settings.IzvodjenjePredstaveCollectionName); 
        }
        #endregion

        /// Vraća sva izvodjenja koja se nalaze u bazi.
        public List<IzvodjenjePredstave> Get() =>
            _izvodjenja.Find(izvodjenjePredstave => true)
                  .ToList();

        /// Vraća sva izvodjenja odredjene predstave.
        public List<IzvodjenjePredstave> GetIzvodjenjaByIdPredstave(string idPredstave) =>
            _izvodjenja.Find(izvodjenje => izvodjenje.IdPredstave == idPredstave).ToList();

        /// Vraća sva izvodjenja koja se igraju odredjenog datuma.
        public List<IzvodjenjePredstave> GetByDate(string datum) =>
            _izvodjenja.Find(izvodjenje => izvodjenje.Datum == datum).ToList();

        /// Vraća koknretno izvodjenje na osnovu vrednosti atributa idIzvodjenja.
        public IzvodjenjePredstave GetByIdIzvodjenja(string idIzvodjenja) =>
            _izvodjenja.Find<IzvodjenjePredstave>(izvodjenje => izvodjenje.IdIzvodjenja == idIzvodjenja)
                  .FirstOrDefault();

        /// Vraća konkretno iyvodjenje na osnovu vrednosti atributa id.
        public IzvodjenjePredstave Get(string id) =>
            _izvodjenja.Find<IzvodjenjePredstave>(izvodjenje => izvodjenje.Id == id)
                  .FirstOrDefault();

        /// Upisuje novo izvodjenje u bazu.
        public IzvodjenjePredstave Create(IzvodjenjePredstave izvodjenje)
        {
            _izvodjenja.InsertOne(izvodjenje);
            return izvodjenje;
        }

        /// Menja postojeću vrednost u bazi.
        /// @param id - id vrednosti koja se menja.
        /// @param newValForIzvodjenje - nova vrednost.
        public void Update(string id, IzvodjenjePredstave newValForIzvodjenje) =>
            _izvodjenja.ReplaceOne(izvodjenje => izvodjenje.Id == id, newValForIzvodjenje);

        /// Briše izvodjenje iz baze.
        public void Remove(IzvodjenjePredstave deleteIzvodjenje) =>
            _izvodjenja.DeleteOne(izvodjenje => izvodjenje.Id == deleteIzvodjenje.Id);

        /// Briše izvodjenje na osnovu vrednosti atributa id.
        public void Remove(string id) =>
            _izvodjenja.DeleteOne(izvodjenje => izvodjenje.Id == id);

    }
}