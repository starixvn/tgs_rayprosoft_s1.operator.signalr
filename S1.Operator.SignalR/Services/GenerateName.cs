using RandomNameGeneratorLibrary;

namespace S1.Operator.SignalR.Services;

public interface IGenerateName
{
    string RandomName();
}
public class GenerateName(IPersonNameGenerator personNameGenerator) : IGenerateName
{
    private static readonly Random Random = new();

    private static readonly List<string> oneWordNames = new List<string>
    {
        "Snickers", "Twitch", "Muffin", "Waffles", "Sprinkles", "Buttons", "Cupcake",
        "Noodle", "Whiskers", "Zigzag", "Flapjack", "Scooter", "Fudge", "Doodle", "Scrappy",
        "Bingo", "Chomper", "Gizmo", "Pudding", "Wobble", "Squiggle", "Fluffy", "Zoom",
        "Puff", "Tater", "Rumble", "Spud", "Pickles"
    };

    public string RandomName()
    {
        var randomType = Random.Next(1, 3);

        return randomType switch
        {
            1 => $"User{Random.Next(1000000, 9999999)}",
            2 => GetFunnyName(),
            _ => personNameGenerator.GenerateRandomFirstAndLastName()
        };
    }

    private static string GetFunnyName()
    {
        var oneWordName = oneWordNames[Random.Next(oneWordNames.Count)];

        if (Random.Next(10) >= 3)
        {
            return oneWordName;
        }
        var randomNumber = Random.Next(10, 100);
        oneWordName += randomNumber;
        return oneWordName;
    }
}
