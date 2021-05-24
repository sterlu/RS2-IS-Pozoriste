using Server.Models;
using System.Collections.Generic;
using MongoDB.Driver;


namespace Server.Services 
{
    public class KorisnikService
    {
        private readonly IMongoCollection<Korisnik> _korisnici;

        #region snippet_KorisnikServiceConstructor
        public KorisnikService(IMyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _korisnici = database.GetCollection<Korisnik>(settings.KorisnikCollectionName); 
        }
        #endregion

        public List<Korisnik> Get() =>
            _korisnici.Find(korisnik => true)
                  .ToList();

        public Korisnik Get(string id) =>
            _korisnici.Find<Korisnik>(korisnik => korisnik.Id == id)
                  .FirstOrDefault();

        public Korisnik GetByUsername(string username) =>
            _korisnici.Find<Korisnik>(korisnik => korisnik.Username == username)
                  .FirstOrDefault();

        public Korisnik Create(Korisnik korisnik)
        {
            _korisnici.InsertOne(korisnik);
            return korisnik;
        }

        public void Update(string id, Korisnik newValForKorisnik) =>
            _korisnici.ReplaceOne(korisnik => korisnik.Id == id, newValForKorisnik);

        public void Remove(Korisnik deleteKorisnik) =>
            _korisnici.DeleteOne(korisnik => korisnik.Id == deleteKorisnik.Id);

        public void Remove(string id) =>
            _korisnici.DeleteOne(korisnik => korisnik.Id == id);
    }
}