using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Server.Models
{
    public class IzvodjenjePredstave
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("sifraPredstave")]
        [JsonProperty("sifraPredstave")]
        public string SifraPredstave { get; set; }

        [BsonElement("brojSale")]
        [JsonProperty("brojSale")]
        public int BrojSale { get; set; }

        [BsonElement("datum")]
        [JsonProperty("datum")]
        public System.DateTime Datum { get; set; }

        [BsonElement("vreme")]
        [JsonProperty("vreme")]
        public System.DateTime Vreme { get; set; }
    }
}