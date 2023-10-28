using RawDeal.Models.Reversals;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Models;
public class Player
{
    private List<Card> Hand { get; } = new List<Card>();
    private List<Card> Arsenal { get; } = new List<Card>();
    private List<Card> Ringside { get; } = new List<Card>();
    private List<Card> RingArea { get; } = new List<Card>();
    private readonly View _view;
    private readonly int _baseFortitude = 0;
    
    public Superstar Superstar { get; private set; }
    public Player(Deck deck, View view)
    {
        Superstar = deck.Superstar;
        _view = view;
        Arsenal.AddRange(deck.Cards);
    }
    public int Fortitude => _baseFortitude + RingArea.Sum(card => int.Parse(card.Damage));
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
        if (!Arsenal.Any()) return;
        Card lastCard = Arsenal.Last();
        Arsenal.Remove(lastCard);
        Hand.Add(lastCard);
    }
    public bool ReceiveDamage(int damageAmount, Card playedCard) => ProcessDamage(damageAmount, playedCard);

    private bool ProcessDamage(int damageAmount, Card playedCard)
    {
        for (int i = 0; i < damageAmount; i++)
        {
            if (!Arsenal.Any()) return true;
            Card lastCard = Arsenal.Last();
            Arsenal.Remove(lastCard);
            Ringside.Add(lastCard);
            _view.ShowCardOverturnByTakingDamage(lastCard.ToString(), i + 1, damageAmount);
            Console.WriteLine(lastCard.GetType());
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