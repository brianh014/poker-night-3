export default class PlayerResult {
  public playerId = '';
  public buyIn = 0;
  public cashOut: number | null = null;
  public loginId = '';
  public name = '';

  constructor (init?:Partial<PlayerResult>) {
    Object.assign(this, init);
  }
}
