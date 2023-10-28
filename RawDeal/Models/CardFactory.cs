namespace RawDeal.Models;

public static class CardFactory
{
    public static Card CreateCard(string title, 
        List<string> types, 
        List<string> subtypes, 
        string fortitude, 
        string damage, 
        string stunValue, 
        string cardEffect)
    {
        Card card = new Card();
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
        return existingCard.Clone();
    }
}