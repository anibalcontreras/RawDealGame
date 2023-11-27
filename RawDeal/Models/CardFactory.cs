namespace RawDeal.Models;

public static class CardFactory
{
    public static Card CreateCardFromExisting(Card existingCard)
    {
        return existingCard.Clone();
    }
}