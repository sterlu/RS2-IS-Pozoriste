import {Izvodjenje} from './izvodjenje';

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
}
