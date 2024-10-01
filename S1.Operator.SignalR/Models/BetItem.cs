namespace S1.Operator.SignalR.Models;

public class BetItem
{
    public string PlayerName { get; set; }

    public string PlayerCode { get; set; }

    public Guid BrandId { get; set; }

    public Guid Id { get; set; }

    public decimal BetAmount { get; set; }

    public int GameId { get; set; }

    public string GameCode { get; set; }

    public int GameType { get; set; }

    public string GameName { get; set; }

    public string GameProviderCode { get; set; }

    public decimal Payout { get; set; }

    public decimal Multiplier { get; set; }

    public DateTime TimeBet { get; set; }

}
