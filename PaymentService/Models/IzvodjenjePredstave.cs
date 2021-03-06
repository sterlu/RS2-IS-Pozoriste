using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace PaymentService.Models
{
    /// Model podataka za izvodjenje predstave.
    public class IzvodjenjePredstave
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("idPredstave")]
        [JsonProperty("idPredstave")]
        public string IdPredstave { get; set; }

        [BsonElement("idIzvodjenja")]
        [JsonProperty("idIzvodjenja")]
        public string IdIzvodjenja { get; set; }

        [BsonElement("brojSale")]
        [JsonProperty("brojSale")]
        public int BrojSale { get; set; }

        [BsonElement("datum")]
        [JsonProperty("datum")] 
        public string Datum { get; set; }

        [BsonElement("vreme")]
        [JsonProperty("vreme")]
        public string Vreme { get; set; }

        [BsonElement("cena")]
        [JsonProperty("cena")]
        public int Cena { get; set; }

        [BsonElement("brojSlobodnihKarata")]
        [JsonProperty("brojSlobodnihKarata")]
        public int BrojSlobodnihKarata { get; set; }
    }
}