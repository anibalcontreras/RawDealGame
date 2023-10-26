using RawDeal.Interfaces;
using RawDeal.Models;
using RawDealView;
using RawDealView.Formatters;
using RawDeal.Models.Reversals;
using RawDeal.Models.Effects;
using RawDeal.Models.Reversals;

public class Player : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    private List<Card> Hand { get; } = new List<Card>();
    private List<Card> Arsenal { get; } = new List<Card>();
    private List<Card> Ringside { get; } = new List<Card>();
    private List<Card> RingArea { get; } = new List<Card>();
    private readonly View _view;
    private int _baseFortitude = 0;
    private ReversalCatalog _reversalCatalog;
    private EffectCatalog _effectCatalog;
    public int Fortitude => _baseFortitude + RingArea.Sum(card => int.Parse(card.Damage));
    public Superstar Superstar { get; private set; }
    public Player(Deck deck, View view)
    {
        Superstar = deck.Superstar;
        Arsenal.AddRange(deck.Cards);
        _view = view;
        _reversalCatalog = new ReversalCatalog(_view);
        _effectCatalog = new EffectCatalog(_view);
    }
    public void RegisterObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers(string message)
    {
        foreach (var observer in _observers)
        {
            observer.Update(message);
        }
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
    public bool ReceiveDamage(int damageAmount, Card playedCard) => ProcessDamage(damageAmount, playedCard);
    public bool ReceiveKaneDamage(int damageAmount) => ProcessDamage(damageAmount, null);

    private bool ProcessDamage(int damageAmount, Card playedCard)
    {
        for (int i = 0; i < damageAmount; i++)
        {
            if (!Arsenal.Any()) return true;

            Card lastCard = Arsenal.Last();
            Arsenal.Remove(lastCard);
            Ringside.Add(lastCard);
            _view.ShowCardOverturnByTakingDamage(lastCard.ToString(), i + 1, damageAmount);

            if (playedCard == null) continue;

            Reversal reversalCard = _reversalCatalog.GetReversalBy(lastCard.Title);
            if (reversalCard != null && reversalCard.Apply(this, this, playedCard))
            {
                Console.WriteLine("Reversamos carta desde el mazo");
                NotifyObservers("CardReversedByDeck");
                return false; // Damage stopped and opponent's turn ended
            }
        }
        return false; // Player has not lost
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
    
    public void OpponentUseReversal(Card cardToUse)
    {
        if (Hand.Contains(cardToUse))
        {
            Console.WriteLine("Agregamos al ringside el reversal a usar");
            Ringside.Add(cardToUse);
            Hand.Remove(cardToUse);
            // _view.SayThatPlayerMustDiscardThisCard(Superstar.Name, CardToUse.Title);
        }
    }
}