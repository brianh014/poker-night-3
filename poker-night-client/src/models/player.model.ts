export default class Player {
  public id = '';
  public loginId = '';
  public name = '';
  public email = '';
  public lastLogIn = '';

  constructor (init?:Partial<Player>) {
    Object.assign(this, init);
  }

  LastLoginFormatted (): string {
    if (!this.lastLogIn) return '';
    return new Date(this.lastLogIn).toLocaleDateString('en-us', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }
}
