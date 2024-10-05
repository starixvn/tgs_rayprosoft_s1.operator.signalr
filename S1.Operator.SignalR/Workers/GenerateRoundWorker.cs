using Microsoft.AspNetCore.SignalR;
using S1.Operator.SignalR.Constants;
using S1.Operator.SignalR.Hubs;
using S1.Operator.SignalR.Models;
using S1.Operator.SignalR.Services;

namespace S1.Operator.SignalR.Workers;

public class GenerateRoundWorker(IHubContext<LastBetHub> hubContext, IGenerateName generateName) : BackgroundService
{
    private static readonly Random Random = new();


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var randomItem = GenerateBetItem();
            await hubContext.Clients.All.SendAsync("ReceiveLastBet", randomItem, cancellationToken: stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private BetItem GenerateBetItem()
    {
        var gameIndex = Random.Next(0, GameStore.records.Count - 1);
        var gameInfo = GameStore.records[gameIndex];
        return new BetItem
        {
            Id = Guid.NewGuid(),
            PlayerName = generateName.RandomName(),
            BetAmount = Random.Next(1, 100),
            GameId = gameInfo.GameId,
            GameCode = gameInfo.GameCode,
            GameType = gameInfo.GameType,
            GameName = gameInfo.GameCode.Replace("game-", "").Replace("-", " "),
            PlayerCode = GenerateRandomCode(12),
            Payout = Random.Next(-100, 1000),
            BrandId = new Guid("8acacf5a-d808-4a55-8302-d832ee3da6dc"),
            GameProviderCode = gameInfo.GameProviderCode,
            Multiplier = (decimal)Math.Round((Random.Next(0, 2) + Random.NextDouble()), 1, MidpointRounding.ToEven),
            TimeBet = DateTime.Now
        };
    }

    private static string GenerateRandomCode(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}