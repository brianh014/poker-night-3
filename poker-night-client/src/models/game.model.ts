import PlayerResult from './playerResult.model';

export default class Game {
  public id = '';
  public date = '';
  public closed = false;
  public buyIn = 0;
  public type = '';
  public players: PlayerResult[] = [];

  constructor (init?:Partial<Game>) {
    Object.assign(this, init);
  }

  DateFormatted (): string {
    if (!this.date) return '';
    return new Date(this.date).toLocaleDateString('en-us', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  PlayersCashedOut (): number {
    if (this.players.length === 0) return 0;
    return this.players.filter(p => p?.cashOut != null).length;
  }

  CashedOut (): number {
    if (this.players.length === 0) return 0;
    return this.players.filter(p => p?.cashOut != null)
      .map(p => p.cashOut)
      .reduce((accumulator, currentValue) => (accumulator || 0) + (currentValue || 0), 0) || 0;
  }

  BoughtIn (): number {
    if (this.players.length === 0) return 0;
    return this.players.filter(p => p?.buyIn != null)
      .map(p => p.buyIn)
      .reduce((accumulator, currentValue) => (accumulator || 0) + (currentValue || 0), 0) || 0;
  }
}
