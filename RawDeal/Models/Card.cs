using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Models;
public class Card : IViewableCardInfo
{
    public string Title { get; set; } = string.Empty;
    public List<string> Types { get; set; } = new List<string>();
    public List<string> Subtypes { get; set; } = new List<string>();

    public string Fortitude { get; set; } = string.Empty;
    public string Damage { get; set; } = string.Empty;
    public string StunValue { get; set; } = string.Empty;
    public string CardEffect { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Title: {Title}\n" +
                $"Types: {string.Join(", ", Types)}\n" +
                $"Subtypes: {string.Join(", ", Subtypes)}\n" +
                $"Fortitude: {Fortitude}\n" +
                $"Damage: {Damage}\n" +
                $"StunValue: {StunValue}\n" +
                $"CardEffect: {CardEffect}\n";
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
    public readonly View _view;

    public Superstar(View view)
    {
        _view = view;
    }

    public override string ToString()
    {
        return $"Name: {Name}\n" +
                $"Logo: {Logo}\n" +
                $"HandSize: {HandSize}\n" +
                $"SuperstarValue: {SuperstarValue}\n" +
                $"SuperstarAbility: {SuperstarAbility}";
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
    public StoneColdSteveAustin(View view) : base(view) { }

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
