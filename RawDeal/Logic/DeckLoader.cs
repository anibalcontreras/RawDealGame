using RawDeal.Exceptions;
using RawDeal.Models;
using RawDealView;
namespace RawDeal.Logic;
public static class DeckLoader
{
    private static readonly Dictionary<string, Card> allAvailableCards = new Dictionary<string, Card>();
    private static readonly Dictionary<string, Superstar> allAvailableSuperstars = new Dictionary<string, Superstar>();
    private const string SuperstarCardSuffix = " (Superstar Card)";

    public static void InitializeDeckLoader(View view)
    {
        LoadAllCards();
        LoadAllSuperstars(view);
    }

    public static Deck LoadDeck(string path)
    {
        ValidateFile(path);
        string[] lines = File.ReadAllLines(path);
        ValidateLines(lines, path);

        return CreateDeckFromLines(lines);
    }

    private static void LoadAllCards()
    {
        List<Card> cards = CardLoader.LoadCardsFromJson();
        foreach (Card card in cards)
        {
            allAvailableCards[card.Title] = card;
        }
    }

    private static void LoadAllSuperstars(View view)
    {
        List<SuperstarData> superstarsData = SuperstarLoader.LoadSuperstarsFromJson();
        foreach (SuperstarData superstarData in superstarsData)
        {
            Superstar superstar = SuperstarFactory.CreateSuperstar(superstarData.Logo, view);
            superstar.Name = superstarData.Name;
            superstar.Logo = superstarData.Logo;
            superstar.HandSize = superstarData.HandSize;
            superstar.SuperstarValue = superstarData.SuperstarValue;
            superstar.SuperstarAbility = superstarData.SuperstarAbility;
            superstar.SuperstarCard = superstarData.SuperstarCard;
            allAvailableSuperstars[superstar.Name] = superstar;
        }
    }


    private static void ValidateFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"El archivo {path} no fue encontrado.");
        }
    }

    private static void ValidateLines(string[] lines, string path)
    {
        if (lines.Length == 0)
        {
            throw new Exception($"El archivo {path} está vacío.");
        }
    }

    private static Deck CreateDeckFromLines(string[] lines)
    {
        Superstar superstar = GetSuperstarByName(lines[0].Replace(SuperstarCardSuffix, ""));
        var deck = new Deck(superstar)
        {
            Cards = new List<Card>()
        };

        AddCardsToDeck(deck, lines[1..]);

        return deck;
    }

    private static Superstar GetSuperstarByName(string name)
    {
        if (allAvailableSuperstars.TryGetValue(name, out var superstar))
        {
            return superstar;
        }
        throw new Exception($"Superstar {name} no encontrado.");
    }

    private static void AddCardsToDeck(Deck deck, string[] cardLines)
    {
        foreach (var cardTitle in cardLines)
        {
            if (allAvailableCards.TryGetValue(cardTitle, out var card))
                deck.Cards.Add(card.Clone());
        }
    }
}