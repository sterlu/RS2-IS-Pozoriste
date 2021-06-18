using System.Collections.Generic;
using Server.Models;

namespace Server.DTO
{
    //TODO: iskomentarisati ovo
    public class KupovinaKarteInternalDto
    {
        public Predstava predstava { get; set; }
        public IzvodjenjePredstave izvodjenje { get; set; }
        public int Kolicina { get; set; }
    }

    public class KupovinaKarteInternalPayloadDTO
    {
        public List<KupovinaKarteInternalDto> kupovine { get; set; }
        public string username { get; set; }
    }
}