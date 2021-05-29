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

        [BsonElement("idIzvodjenja")]
        [JsonProperty("idIzvodjenja")]
        public string IdIzvodjenja { get; set; }

        [BsonElement("sifraPredstave")]
        [JsonProperty("sifraPredstave")]
        public string SifraPredstave { get; set; }


        [BsonElement("nazivPredstave")]
        [JsonProperty("nazivPredstave")]
        public string NazivPredstave { get; set; }

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