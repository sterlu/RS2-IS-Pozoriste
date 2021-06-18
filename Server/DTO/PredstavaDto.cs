using System.Collections.Generic;
using Server.Models;

namespace Server.DTO
{
    /// klasa koja sadrzi sva izvodjenja jedne predstave.
    public class PredstavaDto
    {
        public PredstavaDto(Predstava predstava, List<IzvodjenjePredstave> izvodjenja)
        {
            this.predstava = predstava;
            this.izvodjenja = izvodjenja;
        }

        public Predstava predstava;
        public List<IzvodjenjePredstave> izvodjenja;
    }
}