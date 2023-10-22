using RawDeal;
using RawDeal.Models;
using RawDealView;
using RawDealView.Formatters;
using RawDeal.Models.Reversals;
public class Player
{
    public Superstar Superstar { get; private set; }
    private List<Card> Hand { get; } = new List<Card>();
    private List<Card> Arsenal { get; } = new List<Card>();
    private List<Card> Ringside { get; } = new List<Card>();
    private List<Card> RingArea { get; } = new List<Card>();
    private readonly View _view;

    private int _baseFortitude = 0;
    
    private ReversalCatalog _reversalCatalog;

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

    public Player(Deck deck, View view)
    {
        Superstar = deck.Superstar;
        Arsenal.AddRange(deck.Cards);
        _view = view;
        _reversalCatalog = new ReversalCatalog(_view);
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
    // public enum DamageResult
    // {
    //     FullDamageReceived,
    //     CardReversedFromDeck,
    //     ArsenalDepleted
    // }
    public bool ReceiveDamage(int damageAmount)
    {
        for (int i = 0; i < damageAmount; i++)
        {
            if (Arsenal.Any())
            {
                Card lastCard = Arsenal.Last();
                // var reversalCard = _reversalCatalog.GetReversalBy(lastCard.Title);
                _view.ShowCardOverturnByTakingDamage(lastCard.ToString(), i + 1, damageAmount);
                Arsenal.Remove(lastCard);
                Ringside.Add(lastCard);
                // if (reversalCard != null)
                // {
                //     bool reversed = reversalCard.Apply(this, lastCard);
                //     if (reversed)
                //         return false;
                // }
            }
            else
                return true;
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