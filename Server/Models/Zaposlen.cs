using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Zaposlen
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }
        
        
        [BsonElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        [BsonElement("password")]
        [JsonProperty("password")]
        public string Password { get; set; }
    
        [BsonElement("email")]
        [JsonProperty("email")]
        public string Email { get; set; }
        

        [BsonElement("jmbg")]
        [JsonProperty("jmbg")]
        public string JMBG { get; set; }

        [BsonElement("imePrezime")]
        [JsonProperty("imePrezime")]
        public string ImePrezime { get; private set; }

        [BsonElement("plata")]
        [JsonProperty("plata")]
        public double Plata  { get; set; }
        //public date  DatumZaposlenja { get; set; }
        //public date DatimIstekaUgovora { get; set }
    }
}