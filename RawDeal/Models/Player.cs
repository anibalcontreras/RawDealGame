using RawDealView;
using RawDealView.Formatters;
using RawDeal.Models.Superstars;
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
        List<Card> deckCards = deck.Cards;
        Arsenal.AddRange(deckCards);
    }

    public int Fortitude 
    {
        get
        {
            int totalDamage = CalculateTotalDamageFromRingArea();
            int totalFortitude = _baseFortitude + totalDamage;
            return totalFortitude;
        }
    }

    private int CalculateTotalDamageFromRingArea()
    {
        int totalDamage = 0;
        foreach (Card card in RingArea)
        {
            int cardDamage = int.Parse(card.Damage);
            totalDamage += cardDamage;
        }
        return totalDamage;
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
        string playerName = Superstar.Name;
        int playerFortitude = Fortitude;
        int handCount = Hand.Count;
        int arsenalCount = Arsenal.Count;
        return new PlayerInfo(playerName, playerFortitude, handCount, arsenalCount);
    }
    public bool HasEmptyArsenal()
    {
        return !Arsenal.Any();
    }
}