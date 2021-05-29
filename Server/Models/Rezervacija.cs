using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Rezervacija
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("brojRezervacije")]
        [JsonProperty("brojRezervacije")]
        public int BrojRezervacije { get; set; }

        [BsonElement("brojSale")]
        [JsonProperty("brojSale")]
        public int BrojSale { get; set; } //sala
        
        [BsonElement("datum")]
        [JsonProperty("datum")]
        public System.DateTime Datum { get; set; }

        [BsonElement("vreme")]
        [JsonProperty("vreme")]
        public string Vreme { get; set; }
    }
}