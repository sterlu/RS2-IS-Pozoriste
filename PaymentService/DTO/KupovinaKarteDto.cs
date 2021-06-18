using System.Collections.Generic;
using PaymentService.Models;

namespace PaymentService.DTO
{
    /// Model podataka za kupovinu karte.
    public class KupovinaKarteInternalDTO
    {
        public Predstava predstava { get; set; }
        public IzvodjenjePredstave izvodjenje { get; set; }
        public int Kolicina { get; set; }
    }

    /// Model podataka za kupovinu karata.
    public class KupovinaKarteInternalPayloadDTO
    {
        public List<KupovinaKarteInternalDTO> kupovine { get; set; }
        public string username { get; set; }
    }

}