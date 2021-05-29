using Server.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebPush;

namespace Server.Services
{
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

        public List<PushPretplata> Get() =>
            _pretplate.Find(pretplata => true).ToList();

        public PushPretplata Get(string id) =>
            _pretplate.Find<PushPretplata>(pretplata => pretplata.Id == id).FirstOrDefault();

        public PushPretplata Create(PushPretplata pretplata)
        {
            _pretplate.InsertOne(pretplata);
            return pretplata;
        }

        public void Update(string id, PushPretplata pretplataIn) =>
            _pretplate.ReplaceOne(pretplata => pretplata.Id == id, pretplataIn);

        public void Remove(PushPretplata pretplataIn) =>
            _pretplate.DeleteOne(pretplata => pretplata.Id == pretplataIn.Id);

        public void Remove(string id) => 
            _pretplate.DeleteOne(pretplata => pretplata.Id == id);
        
        public void Obavesti(string idPredstave)
        {
            // TODO: ubaciti ovde obavestavanje mailom za nove predstave iz korisnik kontrolera,
            //       ili prebaciti sve u mailingList u korisnik servisu pozvati ovde
            
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