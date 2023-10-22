using RawDeal.Models;
using System.Text.Json;
using RawDealView;

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
    private static JsonLoader<SuperstarData> _jsonLoader = new JsonLoader<SuperstarData>("data/superstar.json");
    public static List<SuperstarData> LoadSuperstarsFromJson()
    {
        return _jsonLoader.LoadFromJson();
    }
    public static Dictionary<string, Superstar> LoadSuperstarsIntoDictionary(View view)
    {
        List<SuperstarData> superstarsList = LoadSuperstarsFromJson();
        Dictionary<string, Superstar> superstarsDict = new Dictionary<string, Superstar>();
        foreach (var superstarData in superstarsList)
        {
            var superstar = SuperstarFactory.CreateSuperstar(superstarData.Logo, view);
            superstar.Name = superstarData.Name;
            superstar.Logo = superstarData.Logo;
            superstar.HandSize = superstarData.HandSize;
            superstar.SuperstarValue = superstarData.SuperstarValue;
            superstar.SuperstarAbility = superstarData.SuperstarAbility;
            superstar.SuperstarCard = superstarData.SuperstarCard;
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
}