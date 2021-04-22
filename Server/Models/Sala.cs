using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Sala
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("brojSale")]
        [JsonProperty("brojSale")]
        public int BrojSale { get; set; }
        
        [BsonElement("brojMesta")]
        [JsonProperty("brojMesta")]
        public int BrojMesta { get; set; }

        [BsonElement("naziv")]
        [JsonProperty("naziv")]
        public string Naziv { get; set; }
    }
}