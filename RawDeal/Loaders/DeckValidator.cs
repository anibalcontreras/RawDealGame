using RawDeal.Models;
using RawDeal.Models.Superstars;
namespace RawDeal.Loaders;

public static class DeckValidator
{
    private const int DeckSize = 60;
    private const int MaxNonUniqueCards = 3;
    private enum CardSubtype
    {
        Unique,
        SetUp,
        Heel,
        Face
    }
    public static (bool IsValid, List<string> Errors) IsValidDeck(Deck deck, 
        Dictionary<string, Superstar> allAvailableSuperstars)
    {
        List<string> errors = new List<string>();

        if (!HasCorrectCardCount(deck))
        {
            errors.Add("El mazo no tiene el conteo correcto de cartas o falta el Superstar.");
        }

        if (!HasValidRepeatedCards(deck))
        {
            errors.Add("El mazo tiene cartas repetidas inválidas.");
        }

        if (!HasEitherHeelOrFaceCards(deck))
        {
            errors.Add("El mazo tiene ambas cartas, 'heel' y 'face'.");
        }

        if (!SuperstarCanUseExclusiveCards(deck, allAvailableSuperstars))
        {
            errors.Add("El mazo tiene cartas que no pertenecen al Superstar seleccionado.");
        }

        return (errors.Count == 0, errors);
    }

    private static bool HasCorrectCardCount(Deck deck)
    {
        return deck.Cards.Count == DeckSize && !string.IsNullOrWhiteSpace(deck.Superstar.Name);
    }

    private static bool HasValidRepeatedCards(Deck deck)
    {
        var cardGroups = deck.Cards.GroupBy(c => c.Title);

        foreach (var group in cardGroups)
        {
            var card = group.First();
        
            if (card.Subtypes.Contains(CardSubtype.Unique.ToString()) && group.Count() > 1)
            {
                return false;
            }

            if (!card.Subtypes.Contains(CardSubtype.SetUp.ToString()) && group.Count() > MaxNonUniqueCards)
            {
                return false;
            }
        }
        return true;
    }
    private static bool HasEitherHeelOrFaceCards(Deck deck)
    {
        bool hasHeel = deck.Cards.Any(card => card.Subtypes.Contains(CardSubtype.Heel.ToString()));
        bool hasFace = deck.Cards.Any(card => card.Subtypes.Contains(CardSubtype.Face.ToString()));
        return !(hasHeel && hasFace);
    }
    private static bool SuperstarCanUseExclusiveCards(Deck deck,
        Dictionary<string, Superstar> allAvailableSuperstars)
    {
        string superstarLogo = deck.Superstar.Logo;

        foreach (Card card in deck.Cards)
        {
            if (HasInvalidSubtype(card, allAvailableSuperstars, superstarLogo))
            {
                return false;
            }
        }
        return true;
    }

    private static bool HasInvalidSubtype(Card card, 
        Dictionary<string,Superstar> allAvailableSuperstars, 
        string superstarLogo)
    {
        foreach (string subtype in card.Subtypes)
        {
            if (IsInvalidSubtype(subtype, allAvailableSuperstars, superstarLogo))
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsInvalidSubtype(string subtype,
        Dictionary<string, Superstar> allAvailableSuperstars,
        string superstarLogo)
    {
        return allAvailableSuperstars.ContainsKey(subtype) && subtype != superstarLogo;
    }

}   
