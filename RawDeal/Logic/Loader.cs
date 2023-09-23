using RawDeal.Models;
using System.Text.Json;

namespace RawDeal.Logic;
public class JsonLoader<T>
{
    private readonly string _dataPath;

    public JsonLoader(string dataPath)
    {
        _dataPath = dataPath;
    }

    public List<T> LoadFromJson()
    {
        if (File.Exists(_dataPath))
        {
            var json = File.ReadAllText(_dataPath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
        throw new FileNotFoundException($"Cannot find file at {_dataPath}");
    }
}

public static class SuperstarLoader
{
    private static JsonLoader<Superstar> _jsonLoader = new JsonLoader<Superstar>("data/superstar.json");

    public static List<Superstar> LoadSuperstarsFromJson()
    {
        return _jsonLoader.LoadFromJson();
    }

    public static Dictionary<string, Superstar> LoadSuperstarsIntoDictionary()
    {
        var superstarsList = LoadSuperstarsFromJson();
        Dictionary<string, Superstar> superstarsDict = new Dictionary<string, Superstar>();

        foreach (var superstar in superstarsList)
        {
            superstarsDict[superstar.Logo] = superstar;
        }

        return superstarsDict;
    }
}

public static class CardLoader
{
    private static JsonLoader<Card> _jsonLoader = new JsonLoader<Card>("data/cards.json");

    public static List<Card> LoadCardsFromJson()
    {
        return _jsonLoader.LoadFromJson();
    }

    // public static Dictionary<string, Card> LoadCardsIntoDictionary()
    // {
    //     var cardsList = LoadCardsFromJson();
    //     Dictionary<string, Card> cardsDict = new Dictionary<string, Card>();

    //     foreach (var card in cardsList)
    //     {
    //         cardsDict[card.Title] = card;
    //     }
    //     return cardsDict;
    // }
}