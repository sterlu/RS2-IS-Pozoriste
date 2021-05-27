using Server.Models;
using System.Collections.Generic;
using MongoDB.Driver;
using System;

namespace Server.Services
{
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

        public List<IzvodjenjePredstave> Get() =>
            _izvodjenja.Find(izvodjenjePredstave => true)
                  .ToList();

        public List<IzvodjenjePredstave> GetIzvodjenjaBySifraPredstave(string sifraPredstave) =>
            _izvodjenja.Find(izvodjenje => izvodjenje.SifraPredstave == sifraPredstave).ToList();

        public List<IzvodjenjePredstave> GetByDate(DateTime datum) =>
            _izvodjenja.Find(izvodjenje => izvodjenje.Datum == datum).ToList(); 
       
            

        public IzvodjenjePredstave Get(string id) =>
            _izvodjenja.Find<IzvodjenjePredstave>(izvodjenje => izvodjenje.Id == id)
                  .FirstOrDefault();

        public IzvodjenjePredstave Create(IzvodjenjePredstave izvodjenje)
        {
            _izvodjenja.InsertOne(izvodjenje);
            return izvodjenje;
        }

        public void Update(string id, IzvodjenjePredstave newValForIzvodjenje) =>
            _izvodjenja.ReplaceOne(izvodjenje => izvodjenje.Id == id, newValForIzvodjenje);

        public void Remove(IzvodjenjePredstave deleteIzvodjenje) =>
            _izvodjenja.DeleteOne(izvodjenje => izvodjenje.Id == deleteIzvodjenje.Id);

        public void Remove(string id) =>
            _izvodjenja.DeleteOne(izvodjenje => izvodjenje.Id == id);

    }
}