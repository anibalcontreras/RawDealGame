using RawDeal.Models;
using RawDealView;
using RawDealView.Formatters;

public class Player
{
    public Superstar Superstar { get; private set; } = new Superstar();
    private List<Card> Hand { get; } = new List<Card>();
    private List<Card> Arsenal { get; } = new List<Card>();
    private List<Card> Ringside { get; } = new List<Card>();
    private List<Card> RingArea { get; } = new List<Card>();
    private readonly View _view;

    public int Fortitude => RingArea.Sum(card => int.Parse(card.Damage));

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

    public Player(Deck deck, View view)
    {
        Superstar = deck.Superstar;
        Arsenal.AddRange(deck.Cards);
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
        if (Arsenal.Any())
        {
            Card lastCard = Arsenal.Last();
            Arsenal.Remove(lastCard);
            Hand.Add(lastCard);
        }
    }

    public bool ReceiveDamage(int damageAmount)
    {
        for (int i = 0; i < damageAmount; i++)
        {
            if (Arsenal.Any())
            {
                Card lastCard = Arsenal.Last();
                Arsenal.Remove(lastCard);
                Ringside.Add(lastCard);
                _view.ShowCardOverturnByTakingDamage(lastCard.ToString(), i + 1, damageAmount);
            }
            else
            {
                return true;
            }
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
}