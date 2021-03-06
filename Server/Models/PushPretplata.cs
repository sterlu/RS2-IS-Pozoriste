using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Server.Models
{
    /// Model podataka za pretplatu korisnika, kako bi dobijao obaveštenja o predstavama.
    public class PushPretplata
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }
        
        [BsonElement("endpoint")]
        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        [BsonElement("p256dh")]
        [JsonProperty("p256dh")]
        public string P256DH { get; set; }

        [BsonElement("auth")]
        [JsonProperty("auth")]
        public string Auth { get; set; }

        [BsonElement("idPredstave")]
        [JsonProperty("idPredstave")]
        public string IdPredstave { get; set; }

        [BsonElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }
        
        
    }
}
