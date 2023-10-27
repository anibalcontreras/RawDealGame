using RawDeal.Models.Reversals;
namespace RawDeal.Models;

public static class CardFactory
{
    public static Card CreateCard(string title, List<string> types, List<string> subtypes, string fortitude, string damage, string stunValue, string cardEffect)
    {
        Card card;

        if (ReversalCatalog.Instance.Contains(title))
        {
            card = ReversalCatalog.Instance.GetReversalBy(title);
        }
        else
        {
            card = new Card();
        }
        card.Title = title;
        card.Types = types;
        card.Subtypes = subtypes;
        card.Fortitude = fortitude;
        card.Damage = damage;
        card.StunValue = stunValue;
        card.CardEffect = cardEffect;
        return card;
    }

    public static Card CreateCardFromExisting(Card existingCard)
    {
        // return CreateCard(
        //     existingCard.Title,
        //     existingCard.Types,
        //     existingCard.Subtypes,
        //     existingCard.Fortitude,
        //     existingCard.Damage,
        //     existingCard.StunValue,
        //     existingCard.CardEffect
        // );
        return new Card
        {
            Title = existingCard.Title,
            Types = existingCard.Types,
            Subtypes = existingCard.Subtypes,
            Fortitude = existingCard.Fortitude,
            Damage = existingCard.Damage,
            StunValue = existingCard.StunValue,
            CardEffect = existingCard.CardEffect
        };
    }
}