import { Sala } from './sala';

export class Izvodjenje {
  constructor(
    public _datum: Date,
    public predstavaId: string = '',
    public sala: Sala = null,
    public Id: string = null,
    public cena: number = 500,
  ) {}

  get datum(): string {
    return `${this._datum.getDate()}. ${this._datum.getMonth() + 1}. ${this._datum.getFullYear()}.`;
  }

  set datum(value: string) {
    console.log(value);
    if (value) {
      const _value = new Date(value);
      this._datum.setFullYear(_value.getFullYear());
      this._datum.setMonth(_value.getMonth());
      this._datum.setDate(_value.getDate());
    }
  }

  padLeft(str) {
    let _str = str.toString();
    while (_str.length < 2) _str = `0${_str}`
    return _str;
  }

  get vreme(): string {
    return `${this.padLeft(this._datum.getHours())}:${this.padLeft(this._datum.getMinutes())}`;
  }

  set vreme(val: string) {
    this._datum.setHours(+val.split(':')[0]);
    this._datum.setMinutes(+val.split(':')[1]);
  }

  toPayload() {
    console.log('toPayload');
    return {
      Id: this.Id,
      idPredstave: this.predstavaId,
      brojSale: this.sala.brojSale,
      datum: this._datum.toISOString().substr(0, 10),
      vreme: this.vreme,
      cena: this.cena,
    }
  }
}
