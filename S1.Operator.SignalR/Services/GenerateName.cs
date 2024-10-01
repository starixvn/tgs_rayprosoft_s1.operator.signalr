using RandomNameGeneratorLibrary;
using S1.Operator.SignalR.Constants;

namespace S1.Operator.SignalR.Services;

public interface IGenerateName
{
    string RandomName();
}

public class GenerateName(IPersonNameGenerator personNameGenerator) : IGenerateName
{
    private static readonly Random Random = new();



    public string RandomName()
    {
        var randomType = Random.Next(1, 8);

        return randomType switch
        {
            1 => personNameGenerator.GenerateRandomFirstAndLastName(),
            2 => personNameGenerator.GenerateRandomLastName(),
            3 => GetNameWithNumber(),
            4 => PlayerNames.OneWordNames[Random.Next(PlayerNames.OneWordNames.Count)],
            5 => PlayerNames.TwoWordNames[Random.Next(PlayerNames.TwoWordNames.Count)],
            _ => $"User{Random.Next(1000000, 9999999)}"
        };
    }

    private string GetNameWithNumber()
    {
        var oneWordName = personNameGenerator.GenerateRandomLastName();

        return oneWordName + Random.Next(10, 1000);
    }
}
