using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Klijent
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }

        [BsonElement("imePrezime")]
        [JsonProperty("imePrezime")]
        public string ImePrezime { get; set; }

        [BsonElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        [BsonElement("password")]
        [JsonProperty("password")]
        public string Password { get; set; }

        [BsonElement("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        
        
         
    }
}