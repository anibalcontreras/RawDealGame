using RawDealView;
using RawDeal.Models.Superstars;
namespace RawDeal.Loaders;
public static class SuperstarLoader
{
    static readonly string SuperstarDataPath = "data/superstar.json";
    private static JsonLoader<SuperstarData> _jsonLoader = new JsonLoader<SuperstarData>(SuperstarDataPath);
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
