using System.Collections.Generic;
using Server.Models;

namespace Server.DTO
{
    /// Klasa koja predstavlja informacije potrebne za pretplatu korisnika.
    public class PushPretplataDTO
    {
        public string Id { get; set; }
        public string NazivPredstave { get; set; }

        public PushPretplataDTO(string id, string nazivPredstave)
        {
            Id = id;
            NazivPredstave = nazivPredstave;
        }
    }
    /// TODO: iskomentarisati ovo
    public class PushPretplataPayloadDTO
    {
        public List<PushPretplataDTO> push { get; set; }
        public bool email { get; set; }

        public PushPretplataPayloadDTO(List<PushPretplataDTO> push, bool email)
        {
            this.push = push;
            this.email = email;
        }
    }
}