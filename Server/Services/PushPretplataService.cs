using Server.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebPush;

namespace Server.Services
{
    /// Servis koji omogućava čitanje, pisanje, menjanje i brisanje podataka iz tabele PushPretplata..
    public class PushPretplataService
    {
        private readonly IMongoCollection<PushPretplata> _pretplate;
        private VapidDetails _vapidDetails;
        private PredstavaService _predstavaService;

        public PushPretplataService(IMyDatabaseSettings settings, [FromServices] VapidDetails vapidDetails, PredstavaService predstavaService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _pretplate = database.GetCollection<PushPretplata>(settings.PushPretplateCollectionName);

            _vapidDetails = vapidDetails;
            _predstavaService = predstavaService;
        }
        /// Vraća sve pretplate iz baze.

        public List<PushPretplata> Get() =>
            _pretplate.Find(pretplata => true).ToList();

        /// Vraća odredjenu pretplatu.
        public PushPretplata Get(string id) =>
            _pretplate.Find<PushPretplata>(pretplata => pretplata.Id == id).FirstOrDefault();
        
        /// Vraća sve pretplate odredjenog korisnika.
        /// @param username - username korisnika za koga se traže pretplate.
        public List<PushPretplata> GetForUser(string username) =>
            _pretplate.Find<PushPretplata>(pretplata => pretplata.Username == username).ToList();

        /// Upisuje novu pretplatu u bazu.
        public PushPretplata Create(PushPretplata pretplata)
        {
            _pretplate.InsertOne(pretplata);
            return pretplata;
        }

        /// Menja postojeću vrednost u bazi.
        /// @param id - id vrednosti koja se menja.
        /// @param pretplataIn - nova vrednost.
        public void Update(string id, PushPretplata pretplataIn) =>
            _pretplate.ReplaceOne(pretplata => pretplata.Id == id, pretplataIn);

        /// Briše pretplatu iz baze.
        public void Remove(PushPretplata pretplataIn) =>
            _pretplate.DeleteOne(pretplata => pretplata.Id == pretplataIn.Id);

        /// Briše pretplatu iz baze na osnovu vrednosti atributa id.
        public void Remove(string id) => 
            _pretplate.DeleteOne(pretplata => pretplata.Id == id);
        
         /// Briše iz baze pretplatu odredjenog korisnika za odredjenu predstavu.
        public void Remove(string username, string idPredstave) => 
            _pretplate.DeleteOne(pretplata => pretplata.Username == username && pretplata.IdPredstave == idPredstave);
        
        /// Obaveštava korisnika da predstava uskoro kreće sa izvodjenjem.
        public void Obavesti(string idPredstave)
        {
            
            var predstava = _predstavaService.Get(idPredstave); 
            var client = new WebPushClient();
            var sadrzaj =
                "{\"notification\":{\"title\":\"Predstava se uskoro izvodi!\",\"body\":\"Predstava \\\""
                + predstava.NazivPredstave
                + "\\\" za koju ste zatražili obaveštenja uskoro kreće sa izvođenjem!\"}}";

            var subs = _pretplate.Find(pretplata => pretplata.IdPredstave == idPredstave).ToList();
            foreach (var pretplata in subs)
            {
                var _pretplata = new PushSubscription(pretplata.Endpoint, pretplata.P256DH, pretplata.Auth);
                client.SendNotification(_pretplata, sadrzaj, _vapidDetails);
            }
        }
    }
}