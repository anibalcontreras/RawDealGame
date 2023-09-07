using RawDeal.Models;
using RawDealView;
using RawDealView.Formatters;

public class Player
{
    public Superstar Superstar { get; set; } = new Superstar();
    public List<Card> Hand { get; private set; } = new List<Card>();
    public List<Card> Arsenal { get; private set; } = new List<Card>();
    public List<Card> Ringside { get; private set; } = new List<Card>();

    public List<Card> RingArea { get; private set; } = new List<Card>();

    public Player(Deck deck)
    {
        Superstar = deck.Superstar;
        Hand = new List<Card>();
        Arsenal = new List<Card>(deck.Cards);
        Ringside = new List<Card>();
    }

    public List<string> GetFormattedCardsInfo(List<Card> cards)
    {
        return cards.Select(Formatter.CardToString).ToList();
    }

    public PlayerInfo ToPlayerInfo()
    {
        return new PlayerInfo(Superstar.Name, 0, Hand.Count, Arsenal.Count);
        // return new PlayerInfo(Superstar.Name, fortitudeRating, Hand.Count, Arsenal.Count)
    }

    public void DrawCard()
    {
        if (Arsenal.Count > 0)
        {
            Card topCard = Arsenal[Arsenal.Count - 1];
            Arsenal.RemoveAt(Arsenal.Count - 1);
            Hand.Add(topCard);
        }
    }

    public void DiscardCard(Card card)
    {
        Hand.Remove(card);
        Ringside.Add(card);
    }

    public void ApplyDamage(Card card)
    {
        Arsenal.Remove(card);
        Ringside.Add(card);
    }

}