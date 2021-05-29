using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Karta
    {
        public Karta(int cena, string status, string idPredstave, string idRezervacije, string username)
        {
            Cena = cena;
            Status = status;
            IdPredstave = idPredstave;
            IdRezervacije = idRezervacije;
            Username = username;
        }

        [BsonId] // Znaci da je primarni kljuc
        [BsonRepresentation(BsonType.ObjectId)] //PrimarniKljuc je tipa ObjectId, pa da bi smo ga gledali kao string u aplikaciji
        public string Id { get; set; }

        [BsonElement("cena")] 
        [JsonProperty("cena")]
        public int Cena { get; set; }
        
        [BsonElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [BsonElement("idPredstave")]
        [JsonProperty("idPredstave")]
        public string IdPredstave { get; set; }
        
        [BsonElement("idIzvodjenja")]
        [JsonProperty("idIzvodjenja")]
        public string IdIzvodjenja { get; set; }

        [BsonElement("idRezervacije")]
        [JsonProperty("idRezervacije")]
        public string IdRezervacije { get; set; } //rezervacija

        // [BsonElement("id_korisnika")] // za kada se ime kolone(property) u bazi ne poklapa sa imenom promenljive
        // [JsonProperty("id_korisnika")] // ako se koristi NuGet biblioteka Microsoft.AspNetCore.Mvc.NewtonsoftJson -> using Newtonsoft.Json;
        [BsonElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; } //klijent
    }
}
