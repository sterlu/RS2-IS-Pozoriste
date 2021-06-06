import { Izvodjenje } from './izvodjenje';

export class Predstava {
  constructor(
    public nazivPredstave: string,
    public opis: string,
    public status: string,
    public trajanje: number,
    public izvodjenja: Izvodjenje[] = [],
    public Id: string = '',
  ) {
  }

  toPayload() {
    return {
      Id: this.Id,
      nazivPredstave: this.nazivPredstave,
      opis: this.opis,
      status: this.status,
      trajanje: this.trajanje,
    }
  }
}
