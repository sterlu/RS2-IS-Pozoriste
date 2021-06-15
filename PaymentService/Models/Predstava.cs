using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace PaymentService.Models
{
    public class Predstava
    {   
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("sifraPredstave")]
        [JsonProperty("sifraPredstave")]
        public string SifraPredstave { get; set; }

        [BsonElement("nazivPredstave")]
        [JsonProperty("nazivPredstave")]
        public string NazivPredstave { get; set; }

        [BsonElement("opis")]
        [JsonProperty("opis")]
        public string Opis { get; set; }

        [BsonElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [BsonElement("trajanje")]
        [JsonProperty("trajanje")]
        public int Trajanje { get; set; } 
        
    }
}