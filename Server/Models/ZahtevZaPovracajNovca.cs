using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Globalization;

/**
 * TODO Obrisati
 */
namespace Server.Models
{
    public class ZahtevZaPovracajNovca
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("brojZahteva")]
        [JsonProperty("brojZahteva")]
        public int BrojZahteva { get; set; }

        [BsonElement("razlog")]
        [JsonProperty("razlog")]
        public string Razlog { get; set; }

        [BsonElement("kod")]
        [JsonProperty("kod")]
        public string Kod { get; set; }
        
        [BsonElement("datum")]
        [JsonProperty("datum")]
        public System.DateTime Datum { get; set;}
    }
}