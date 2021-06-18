using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Server.Models
{
    /// Model podataka za korisnika.
    public class Korisnik
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; } 

        [BsonElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        [BsonElement("passwordHash")]
        [JsonProperty("passwordHash")]
        public byte[] PasswordHash { get; set; }

        [BsonElement("passwordSalt")]
        [JsonProperty("passwordSalt")]
        public byte[] PasswordSalt { get; set; }

        [BsonElement("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [BsonElement("tip")]
        [JsonProperty("tip")]
        public string Tip { get; set; }

        [BsonElement("emailObavestenja")]
        [JsonProperty("emailObavestenja")]
        public bool  EmailObavestenja { get; set; }
    }
}