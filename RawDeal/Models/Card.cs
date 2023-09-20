using RawDealView;
using RawDealView.Formatters;
using RawDeal.Logic;

namespace RawDeal.Models;

public static class CardRepository
{
    private static readonly Dictionary<string, Card> allAvailableCards = new Dictionary<string, Card>();

    public static void LoadAllCards()
    {
        var cards = CardLoader.LoadCardsFromJson(); 
        foreach (var card in cards)
        {
            allAvailableCards[card.Title] = card;
        }
    }

    public static Card GetCardByTitle(string title)
    {
        if (allAvailableCards.TryGetValue(title, out var card))
        {
            return card;
        }
        return null;
    }
}

public class Card : IViewableCardInfo
{
    public string Title { get; set; } = string.Empty;
    public List<string> Types { get; set; } = new List<string>();
    public List<string> Subtypes { get; set; } = new List<string>();

    public string Fortitude { get; set; } = string.Empty;
    public string Damage { get; set; } = string.Empty;
    public string StunValue { get; set; } = string.Empty;
    public string CardEffect { get; set; } = string.Empty;
}

public static class SuperstarRepository
{
    private static readonly Dictionary<string, Superstar> allAvailableSuperstars = new Dictionary<string, Superstar>();

    public static void LoadAllSuperstars(View view)
    {
        var rawSuperstars = SuperstarLoader.LoadSuperstarsIntoDictionary();
        foreach (var entry in rawSuperstars)
        {
            allAvailableSuperstars[entry.Key] = Superstar.CreateFromLogo(entry.Key, view);
        }
    }

    public static Superstar GetSuperstarByLogo(string logo)
    {
        if (allAvailableSuperstars.TryGetValue(logo, out var superstar))
        {
            return superstar;
        }
        return null;
    }
}


public class Superstar
{
    protected bool hasUsedAbilityThisTurn = false;
    public string Name { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public int HandSize { get; set; } = 0;
    public int SuperstarValue { get; set; } = 0;
    public string SuperstarAbility { get; set; } = string.Empty;
    public Card SuperstarCard { get; set; } = new Card();
    public View _view;


    public static Superstar CreateFromLogo(string logo, View view)
    {
        switch (logo)
        {
            case "StoneCold":
                return new StoneColdSteveAustin();
            default:
                Superstar superstar = new Superstar();
                return superstar;
        }
    }

    public virtual void ActivateSuperstarAbility(Player currentPlayer, Player opponentPlayer)
    {
        // La implementación básica puede no hacer nada.
    }

    public virtual void ResetAbility()
    {
        hasUsedAbilityThisTurn = false;
    }   
}

public class StoneColdSteveAustin : Superstar
{

    public override void ActivateSuperstarAbility(Player currentPlayer, Player opponentPlayer)
    {
        if (!hasUsedAbilityThisTurn && currentPlayer.Arsenal.Count > 0)
        {
            _view.SayThatPlayerIsGoingToUseHisAbility(currentPlayer.Superstar.Name, currentPlayer.Superstar.SuperstarAbility);

            currentPlayer.DrawCard();
            _view.SayThatPlayerDrawCards(currentPlayer.Superstar.Name, 1); 

            // Elige una carta de la mano y pónla al fondo del arsenal.
            int cardIndexToReturn = _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(currentPlayer.Superstar.Name, currentPlayer.FormattedHand);
            
            Card cardToPlaceAtBottom = currentPlayer.Hand[cardIndexToReturn];

            currentPlayer.Hand.RemoveAt(cardIndexToReturn);
            currentPlayer.Arsenal.Insert(0, cardToPlaceAtBottom);

            hasUsedAbilityThisTurn = true;
        }
    }
}
