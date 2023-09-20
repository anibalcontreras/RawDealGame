using System.ComponentModel;
using RawDeal.Models;
using RawDealView;

namespace RawDeal.Logic;
public static class DeckLoader
{
    private const string SuperstarCardSuffix = " (Superstar Card)";
    public static View CurrentView { get; set; }

    public static void InitializeDeckLoader(View view)
    {
        CurrentView = view;
        CardRepository.LoadAllCards();
        SuperstarRepository.LoadAllSuperstars(CurrentView);
    }

    public static Deck LoadDeck(string path)
    {
        ValidateFile(path);
        var lines = File.ReadAllLines(path);
        ValidateLines(lines, path);

        return CreateDeckFromLines(lines);
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
        var deck = new Deck();

        AddSuperstarToDeck(deck, lines[0].Replace(SuperstarCardSuffix, ""));
        AddCardsToDeck(deck, lines[1..]);

        return deck;
    }

    private static void AddSuperstarToDeck(Deck deck, string superstarLogo)
    {
        var superstar = SuperstarRepository.GetSuperstarByLogo(superstarLogo);
        if (superstar != null)
        {
            deck.AddSuperstar(superstar);
        }
        else
        {
            Console.WriteLine($"Advertencia: El Superstar con el logo '{superstarLogo}' no se encontró.");
        }
    }

    private static void AddCardsToDeck(Deck deck, string[] cardLines)
    {
        foreach (var cardTitle in cardLines)
        {
            var card = CardRepository.GetCardByTitle(cardTitle);
            if (card != null)
            {
                deck.AddCard(card);
            }
            else
            {
                Console.WriteLine($"Advertencia: La card '{cardTitle}' no se encontró en las cartas disponibles.");
            }
        }
    }


}