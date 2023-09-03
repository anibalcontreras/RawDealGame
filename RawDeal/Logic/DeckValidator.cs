using RawDeal.Models;

namespace RawDeal.Logic;

public static class DeckValidator
{
    public static (bool IsValid, List<string> Errors) IsValidDeck(Deck deck, Dictionary<string, Superstar> allAvailableSuperstars)
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
        return deck.Cards.Count == 60 && !string.IsNullOrWhiteSpace(deck.Superstar.Name);
    }

    private static bool HasValidRepeatedCards(Deck deck)
    {
        var cardGroups = deck.Cards.GroupBy(c => c.Title);

        foreach (var group in cardGroups)
        {
            var card = group.First();
            
            if (card.Subtypes.Contains("Unique") && group.Count() > 1)
            {
                return false;
            }

            if (!card.Subtypes.Contains("SetUp") && group.Count() > 3)
            {
                return false;
            }
        }

        return true;
    }

    private static bool HasEitherHeelOrFaceCards(Deck deck)
    {
        bool hasHeel = deck.Cards.Any(card => card.Subtypes.Contains("Heel"));
        bool hasFace = deck.Cards.Any(card => card.Subtypes.Contains("Face"));

        return !(hasHeel && hasFace);
    }
    
    private static bool SuperstarCanUseExclusiveCards(Deck deck, Dictionary<string, Superstar> allAvailableSuperstars)
    {
        string superstarLogo = deck.Superstar.Logo;

        foreach (var card in deck.Cards)
        {
            foreach (var subtype in card.Subtypes)
            {
                if (allAvailableSuperstars.ContainsKey(subtype))
                {
                    if (subtype != superstarLogo)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}   
