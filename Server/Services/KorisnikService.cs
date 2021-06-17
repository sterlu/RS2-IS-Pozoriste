using Server.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Driver;
using Server.DTO;


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

        public List<Korisnik> MailingList() =>
            _korisnici.Find<Korisnik>(korisnik => korisnik.EmailObavestenja == true)
                  .ToList();

        public Korisnik Create(Korisnik korisnik)
        {
            _korisnici.InsertOne(korisnik);
            return korisnik;
        }

        public Korisnik Register(string username, string password, string email, bool obavestenja,  string tip = "korisnik")
        {
            using var hmac = new HMACSHA512();

            var korisnik = new Korisnik {
                Username = username, 
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key, 
                Email = email, 
                Tip = tip, 
                EmailObavestenja = obavestenja

            };

            return Create(korisnik);
        }

        public void Update(string id, Korisnik newValForKorisnik) =>
            _korisnici.ReplaceOne(korisnik => korisnik.Id == id, newValForKorisnik);

        public void UpdateObavestenja(string username)
        {
            Korisnik korisnik = GetByUsername(username);
            korisnik.EmailObavestenja = !korisnik.EmailObavestenja;
            _korisnici.ReplaceOne(k => k.Username == username, korisnik);
        }

        public void Remove(Korisnik deleteKorisnik) =>
            _korisnici.DeleteOne(korisnik => korisnik.Id == deleteKorisnik.Id);

        public void Remove(string id) =>
            _korisnici.DeleteOne(korisnik => korisnik.Id == id);
    }
}