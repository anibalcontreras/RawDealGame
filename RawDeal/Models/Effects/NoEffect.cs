using RawDealView;
namespace RawDeal.Models.Effects;

public class NoEffect : Effect
{
    public NoEffect(View view) : base(view)
    {
    }
    public override bool Apply(Player player, Player opponent, Card playedCard)
    {
        int damage = CalculateCardDamage(playedCard);
        AnnounceDamageToOpponent(damage, opponent);
        bool hasLost = ApplyCardDamageToOpponent(damage, opponent, playedCard);
        if (!hasLost)
            ApplyCardEffect(player, playedCard);
        return hasLost;
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
    
    private bool ApplyCardDamageToOpponent(int cardDamage, Player opponent, Card playedCard)
    {
        int actualDamage = opponent.Superstar.CalculateDamage(cardDamage);
        return opponent.ReceiveDamage(actualDamage, playedCard);
    }
    
    private void ApplyCardEffect(Player player, Card playedCard)
    {
        int indexOfCardInHand = player.GetHand().FindIndex(card => ReferenceEquals(card, playedCard));
        player.ApplyDamage(indexOfCardInHand);
    }
}