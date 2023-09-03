using RawDeal.Models;

namespace RawDeal.Logic;
public static class DeckLoader
{
    private static Dictionary<string, Card> allAvailableCards = new Dictionary<string, Card>();
    private static Dictionary<string, Superstar> allAvailableSuperstars = new Dictionary<string, Superstar>();

    public static void InitializeDeckLoader()
    {
        var cards = CardLoader.LoadCardsFromJson();
        var superstars = SuperstarLoader.LoadSuperstarsFromJson();

        foreach (var card in cards)
        {
            allAvailableCards[card.Title] = card;
        }

        foreach (var superstar in superstars)
        {
            allAvailableSuperstars[superstar.Name] = superstar;
        }
    }
    public static Deck LoadDeck(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"El archivo {path} no fue encontrado.");

        var lines = File.ReadAllLines(path);
        if (lines.Length == 0)
            throw new Exception($"El archivo {path} está vacío.");

        var deck = new Deck
        {
            Superstar = GetSuperstarByName(lines[0].Replace(" (Superstar Card)", "")),
            Cards = new List<Card>()
        };

        for (int i = 1; i < lines.Length; i++)
        {
            var cardTitle = lines[i];
            if (allAvailableCards.ContainsKey(cardTitle))
            {
                deck.Cards.Add(allAvailableCards[cardTitle]);
            }
            else
            {
                Console.WriteLine($"Advertencia: La card '{cardTitle}' no se encontró en las cartas disponibles.");
            }
        }
        return deck;
    }

    private static Superstar GetSuperstarByName(string name)
    {
        if (allAvailableSuperstars.ContainsKey(name))
        {
            return allAvailableSuperstars[name];
        }
        else
        {
            throw new Exception($"Superstar {name} no encontrado.");
        }
    }
}
