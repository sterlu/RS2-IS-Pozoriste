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
        [BsonId] // Znaci da je primarni kljuc
        [BsonRepresentation(BsonType.ObjectId)] //PrimarniKljuc je tipa ObjectId, pa da bi smo ga gledali kao string u aplikaciji
        public string Id { get; set; }

        [BsonElement("kod")]
        [JsonProperty("kod")]
        public string Kod { get; set; }

        [BsonElement("cena")]
        [JsonProperty("cena")]
        public int Cena { get; set; }

        [BsonElement("tip")]
        [JsonProperty("tip")]
        public string Tip { get; set; }

        [BsonElement("sifraPredstave")]
        [JsonProperty("sifraPredstave")]
        public string SifraPredstave { get; set; } //predstava

        [BsonElement("brojRezervacije")]
        [JsonProperty("brojRezervacije")]
        public int BrojRezervacije { get; set; } //rezervacija

        // [BsonElement("id_korisnika")] // za kada se ime kolone(property) u bazi ne poklapa sa imenom promenljive
        // [JsonProperty("id_korisnika")] // ako se koristi NuGet biblioteka Microsoft.AspNetCore.Mvc.NewtonsoftJson -> using Newtonsoft.Json;
        [BsonElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; } //klijent
    }
}
