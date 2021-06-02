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
        
        [BsonElement("idPredstave")]
        [JsonProperty("idPredstave")]
        public string IdPredstave { get; set; }

        [BsonElement("brojSale")]
        [JsonProperty("brojSale")]
        public int BrojSale { get; set; }

        [BsonElement("datum")]
        [JsonProperty("datum")]
        public string Datum { get; set; }

        [BsonElement("vreme")]
        [JsonProperty("vreme")]
        public string Vreme { get; set; }
    }
}