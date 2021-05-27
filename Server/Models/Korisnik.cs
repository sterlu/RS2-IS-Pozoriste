using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Server.Models
{
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

        // treba promeniti naziv atributa
        // trebalo bi da vrednost bude da ako korisnik zeli da se nadje na reklamnoj mailing listi
        [BsonElement("emailObavestenja")]
        [JsonProperty("emailObavestenja")]
        public string  EmailObavestenja { get; set; }
    }
}