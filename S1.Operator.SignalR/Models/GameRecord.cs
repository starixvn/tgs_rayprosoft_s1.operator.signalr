namespace S1.Operator.SignalR.Models;

public class GameRecord(int gameId, string gameProviderCode, int gameType, string gameCode)
{
    public int GameId { get; set; } = gameId;
    public string GameProviderCode { get; set; } = gameProviderCode;
    public int GameType { get; set; } = gameType;
    public string GameCode { get; set; } = gameCode;
}
