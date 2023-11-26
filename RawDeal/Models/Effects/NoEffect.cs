using RawDeal.Controllers;
using RawDeal.Exceptions;
using RawDealView;
namespace RawDeal.Models.Effects;

public class NoEffect : Effect
{
    private readonly PlayerActionsController _playerActionsController;
    private readonly CardPlayController _cardPlayController;
    public NoEffect(View view) : base(view)
    {
        _playerActionsController = new PlayerActionsController(view);
    }
    public override bool Apply(Player player, Player opponent, Card playedCard)
    {
        int damage = CalculateCardDamage(playedCard);
        AnnounceDamageToOpponent(damage, opponent);
        try
        {
            bool hasLost = ApplyCardDamageToOpponent(damage, opponent, playedCard, player);
            Console.WriteLine("La playedCard es: " + playedCard.Title);
            Console.WriteLine("El damage de esta carta es " + playedCard.Damage);
            ApplyCardEffect(player, playedCard);
            return hasLost;
        }
        catch (CardReversedButGameContinuesException)
        {
            ApplyCardEffect(player, playedCard);
            return false;
        }
    }

    private int CalculateCardDamage(Card card)
    {
        return int.Parse(card.Damage);
    }
    private void AnnounceDamageToOpponent(int cardDamage, Player opponent)
    { 
        int actualDamage = opponent.Superstar.CalculateDamage(cardDamage);
        if (actualDamage > 0)
        {
            _view.SayThatSuperstarWillTakeSomeDamage(opponent.Superstar.Name, actualDamage);
        }
    }
    private bool ApplyCardDamageToOpponent(int cardDamage, Player opponent, Card playedCard, Player player)
    {
        int actualDamage = opponent.Superstar.CalculateDamage(cardDamage);
        
        return _playerActionsController.ReceiveDamage(opponent, actualDamage, playedCard, player);
    }
    private void ApplyCardEffect(Player player, Card playedCard)
    {
        int indexOfCardInHand = player.GetHand().FindIndex(card => ReferenceEquals(card, playedCard));
        _playerActionsController.ApplyDamage(player, indexOfCardInHand);
    }
}