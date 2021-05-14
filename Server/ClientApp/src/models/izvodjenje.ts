import {Sala} from './sala';

export class Izvodjenje {
  constructor(
    public _datum: Date,
    public predstavaId: string = '',
    public sala: Sala = null,
  ) {
  }

  get datum(): string {
    return `${this._datum.getDate()}. ${this._datum.getMonth() + 1}. ${this._datum.getFullYear()}.`;
  }

  set datum(value: string) {
    if (value) {
      const _value = new Date(value);
      this._datum.setFullYear(_value.getFullYear());
      this._datum.setMonth(_value.getMonth());
      this._datum.setDate(_value.getDate());
    }
  }

  get vreme(): string {
    return `${this._datum.getHours()}:${this._datum.getMinutes()}`;
  }

  set vreme(val: string) {
    this._datum.setHours(+val.split(':')[0]);
    this._datum.setMinutes(+val.split(':')[1]);
  }
}
