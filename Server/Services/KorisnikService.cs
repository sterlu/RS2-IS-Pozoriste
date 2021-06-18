using Server.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Driver;
using Server.DTO;


namespace Server.Services 
{
    /// Servis koji omogućava čitanje, pisanje, menjanje i brisanje podataka iz tabele Korisnik.
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

        /// Vraća sve korisnike iz tabele.
        public List<Korisnik> Get() =>
            _korisnici.Find(korisnik => true)
                  .ToList();

        /// Vraća korisnika na osnovu vrednosti atributa id.
        public Korisnik Get(string id) =>
            _korisnici.Find<Korisnik>(korisnik => korisnik.Id == id)
                  .FirstOrDefault();

        /// Vraća korisnika na osnovu vrednosti atributa username.
        public Korisnik GetByUsername(string username) =>
            _korisnici.Find<Korisnik>(korisnik => korisnik.Username == username)
                  .FirstOrDefault();

        /// Vraća sve korisnike koji žele da primaju obaveštenja o predstavama elektronskom poštom.
        public List<Korisnik> MailingList() =>
            _korisnici.Find<Korisnik>(korisnik => korisnik.EmailObavestenja == true)
                  .ToList();

        /// Upisuje novog korisnika u bazu.
        public Korisnik Create(Korisnik korisnik)
        {
            _korisnici.InsertOne(korisnik);
            return korisnik;
        }

        /// Registracija korisnika.
        /// Upisuje novog korisnika u bazu uz heširanje lozinke.
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

        /// Menja postojeću vrednost u bazi.
        /// @param id - id vrednosti koja se menja.
        /// @param newValForKorisnik - nova vrednost.
        public void Update(string id, Korisnik newValForKorisnik) =>
            _korisnici.ReplaceOne(korisnik => korisnik.Id == id, newValForKorisnik);

        /// Menja postojeću vrednost atributa EmailObavestenja na suprotnu od postojeće. 
        /// @param username - username koirsnika za koga se menja vrednost atributa.
        public void UpdateObavestenja(string username)
        {
            Korisnik korisnik = GetByUsername(username);
            korisnik.EmailObavestenja = !korisnik.EmailObavestenja;
            _korisnici.ReplaceOne(k => k.Username == username, korisnik);
        }

        /// Briše korisnika iz baze.
        public void Remove(Korisnik deleteKorisnik) =>
            _korisnici.DeleteOne(korisnik => korisnik.Id == deleteKorisnik.Id);

        /// Briše korisnika iz baze na osnovu vrednosti atributa id.
        public void Remove(string id) =>
            _korisnici.DeleteOne(korisnik => korisnik.Id == id);
    }
}