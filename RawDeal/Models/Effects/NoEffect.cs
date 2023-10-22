using RawDealView;
namespace RawDeal.Models.Effects;

public class NoEffect : Effect
{
    public NoEffect(View view) : base(view)
    {
    }
    public override bool Apply(Player player, Player opponent, Card card)
    {
        // Lógica de daño estándar
        int damage = CalculateCardDamage(card);
        // Console.WriteLine("EL daño es" + damage);
        AnnounceDamageToOpponent(damage, opponent);
        bool hasLost = ApplyCardDamageToOpponent(damage, opponent);
        if (!hasLost)
            ApplyCardEffect(player, card);
        return hasLost;
    }

    private int CalculateCardDamage(Card card)
    {
        return int.Parse(card.Damage);
    }

    private void AnnounceDamageToOpponent(int cardDamage, Player opponent)
    {
        int actualDamage = (opponent.Superstar.Logo == "Mankind") ? cardDamage - 1 : cardDamage;
        if (actualDamage > 0)
            _view.SayThatSuperstarWillTakeSomeDamage(opponent.Superstar.Name, actualDamage);
    }
    private bool ApplyCardDamageToOpponent(int cardDamage, Player opponent)
    {
        if (opponent.Superstar.Logo == "Mankind")
            return opponent.ReceiveDamage(cardDamage - 1);
        return opponent.ReceiveDamage(cardDamage);
    }

    private void ApplyCardEffect(Player player, Card playedCard)
    {
        int indexOfCardInHand = player.GetHand().FindIndex(card => ReferenceEquals(card, playedCard));
        player.ApplyDamage(indexOfCardInHand);
    }
}