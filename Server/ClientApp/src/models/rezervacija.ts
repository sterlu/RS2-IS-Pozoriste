import { Predstava } from "./predstava";
import { Izvodjenje } from "./izvodjenje";

export class Rezervacija {
  constructor(
    public predstava: Predstava,
    public izvodjenje: Izvodjenje,
    public kolicina: number,
  ) {
  }
}
