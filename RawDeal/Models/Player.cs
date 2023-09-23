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

    private int _fortitude;

    public int Fortitude
    {
        get { return RingArea.Sum(card => int.Parse(card.Damage)); }
        private set { _fortitude = value; }
    }

    private readonly View _view;

    public Player(Deck deck, View view)
    {
        Superstar = deck.Superstar;
        Hand = new List<Card>();
        Arsenal = new List<Card>(deck.Cards);
        Ringside = new List<Card>();
        _view = view;
    }

    public List<string> GetFormattedCardsInfo(List<Card> cards)
    {
        return cards.Select(Formatter.CardToString).ToList();
    }

    public PlayerInfo ToPlayerInfo()
    {
        return new PlayerInfo(Superstar.Name, Fortitude, Hand.Count, Arsenal.Count);
    }

    public void DrawCard()
    {
        if (Arsenal.Count > 0)
        {
            Card lastCard = Arsenal[Arsenal.Count - 1];
            Arsenal.RemoveAt(Arsenal.Count - 1);
            Hand.Add(lastCard);
        }
    }

    public void ReceiveDamage(int damageAmount)
    {
        for (int i = 0; i < damageAmount; i++)
        {
            if (Arsenal.Count > 0)
            {
                Card lastCard = Arsenal[Arsenal.Count - 1];
                Arsenal.RemoveAt(Arsenal.Count - 1);
                Ringside.Add(lastCard);
                _view.ShowCardOverturnByTakingDamage(lastCard.ToString(), i + 1, damageAmount);
            }
        }
    }

    public void ApplyDamage(int cardIndex)
    {
        Card cardToApply = Hand[cardIndex];
        Hand.RemoveAt(cardIndex);
        RingArea.Add(cardToApply);
    }
}