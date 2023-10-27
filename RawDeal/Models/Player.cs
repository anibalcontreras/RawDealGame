using RawDeal.Models;
using RawDealView;
using RawDealView.Formatters;

public class Player
{
    private List<Card> Hand { get; } = new List<Card>();
    private List<Card> Arsenal { get; } = new List<Card>();
    private List<Card> Ringside { get; } = new List<Card>();
    private List<Card> RingArea { get; } = new List<Card>();
    private readonly View _view;
    private int _baseFortitude = 0;
    public int Fortitude => _baseFortitude + RingArea.Sum(card => int.Parse(card.Damage));
    public Superstar Superstar { get; private set; }
    public Player(Deck deck, View view)
    {
        Superstar = deck.Superstar;
        Arsenal.AddRange(deck.Cards);
        _view = view;
    }
    public List<Card> GetHand()
    {
        return Hand;
    }

    public List<Card> GetRingArea()
    {
        return RingArea;
    }

    public List<Card> GetRingside()
    {
        return Ringside;
    }
    
    public List<Card> GetArsenal()
    {
        return Arsenal;
    }
    
    public List<string> GetFormattedCardsInfo(List<Card> cards)
    {
        return cards.Select(Formatter.CardToString).ToList();
    }

    public PlayerInfo PlayerInfo()
    {
        return new PlayerInfo(Superstar.Name, Fortitude, Hand.Count, Arsenal.Count);
    }

    public void DrawCard()
    {
        if (Arsenal.Any())
        {
            Card lastCard = Arsenal.Last();
            Arsenal.Remove(lastCard);
            Hand.Add(lastCard);
        }
    }
    public bool ReceiveDamage(int damageAmount) => ProcessDamage(damageAmount);

    private bool ProcessDamage(int damageAmount)
    {
        for (int i = 0; i < damageAmount; i++)
        {
            if (!Arsenal.Any()) return true;
            Card lastCard = Arsenal.Last();
            Arsenal.Remove(lastCard);
            Ringside.Add(lastCard);
            _view.ShowCardOverturnByTakingDamage(lastCard.ToString(), i + 1, damageAmount);
        }
        return false;
    }
    public void ApplyDamage(int cardIndex)
    {
        Card cardToApply = Hand[cardIndex];
        Hand.RemoveAt(cardIndex);
        RingArea.Add(cardToApply);
    }
    public bool HasEmptyArsenal()
    {
        return !Arsenal.Any();
    }

    public void DiscardCard(Card cardToDiscard)
    {
        if (Hand.Contains(cardToDiscard))
        {
            Ringside.Add(cardToDiscard);
            Hand.Remove(cardToDiscard);
            _view.SayThatPlayerMustDiscardThisCard(Superstar.Name, cardToDiscard.Title);
        }
    }
}