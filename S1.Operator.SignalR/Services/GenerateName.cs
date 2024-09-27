using RandomNameGeneratorLibrary;

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
        var randomType = Random.Next(1, 3);

        if (randomType / 2 != 0)
        {
            return $"User{Random.Next(1000000, 9999999)}";
        }

        return personNameGenerator.GenerateRandomFirstAndLastName();
    }
}
