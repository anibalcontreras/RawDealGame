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
    
    public bool HasEmptyArsenal()
    {
        return !Arsenal.Any();
    }
}