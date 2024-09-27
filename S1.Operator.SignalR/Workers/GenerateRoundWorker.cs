using Microsoft.AspNetCore.SignalR;
using RandomNameGeneratorLibrary;
using S1.Operator.SignalR.Hubs;
using S1.Operator.SignalR.Models;

namespace S1.Operator.SignalR.Workers;

public class GenerateRoundWorker(IHubContext<LastBetHub> hubContext, IPersonNameGenerator personNameGenerator) : BackgroundService
{
    private static readonly Random Random = new Random();


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var randomItem = GenerateBetItem();
            await hubContext.Clients.Group(randomItem.BrandId.ToString()).SendAsync("ReceiveLastBet", randomItem, cancellationToken: stoppingToken);
            await Task.Delay(600, stoppingToken);
        }
    }

    private BetItem GenerateBetItem()
    {
        return new BetItem
        {
            Id = Guid.NewGuid(),
            PlayerName = GenerateName(),
            BetAmount = Random.Next(1, 100),
            GameId = int.Parse($"74000{Random.Next(1, 99)}"),
            GameName = GenerateRandomCode(6),
            PlayerCode = GenerateRandomCode(12),
            Payout = Random.Next(-100, 1000),
            BrandId = new Guid("8acacf5a-d808-4a55-8302-d832ee3da6dc"),
            GameProviderCode = GenerateRandomCode(6),
            Multiplier = (decimal)Math.Round((Random.Next(0, 2) + Random.NextDouble()), 1, MidpointRounding.ToEven)
        };
    }

    private string GenerateName()
    {
        var randomType = Random.Next(1, 3);

        if (randomType / 2 != 0)
        {
            return $"User{Random.Next(1000000, 9999999)}";
        }
        return personNameGenerator.GenerateRandomFirstAndLastName();
    }

    private static string GenerateRandomCode(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}