using System.Diagnostics;
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
            // Intenta convertir el Damage a un entero y sumarlo al total.
            // Si no es un número, se suma 0.
            bool isParsed = int.TryParse(card.Damage, out int cardDamage);
            if (isParsed)
            {
                totalDamage += cardDamage;
            }
            else
            {
                // Si no es un número válido, se asume que el daño es 0.
                // Esto manejará el caso de "#" o cualquier otro valor no entero.
                totalDamage += 0;
            }
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