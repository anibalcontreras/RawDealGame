using RawDeal.Models;
namespace RawDeal.Loaders;

public static class CardLoader
{
    private static readonly JsonLoader<Card> JsonLoader = new JsonLoader<Card>("data/cards.json");
    public static List<Card> LoadCardsFromJson()
    {
        return JsonLoader.LoadFromJson();
    }
}