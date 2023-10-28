using RawDealView;
namespace RawDeal.Models.Superstars;
public class Jericho : Superstar
{
    private readonly View _view;
    
    public Jericho(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.InMenu;
    }
    public override bool CanUseAbility(Player player)
    {
        int handCount = player.GetHand().Count;
        return !HasUsedAbility && handCount >= 1;
    }

    public override void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
        if (activationTime == ActivationMoment && !HasUsedAbility && CanUseAbility(player))
            UseAbility(player, opponent);
    }

    private void UseAbility(Player player, Player opponent)
    {
        string playerName = player.Superstar.Name;
        string superstarAbility = player.Superstar.SuperstarAbility;

        _view.SayThatPlayerIsGoingToUseHisAbility(playerName, superstarAbility);
        DiscardCard(player);
        OpponentDiscardCard(opponent);
        MarkAbilityAsUsed();
    }

    private void DiscardCard(Player player)
    {
        List<Card> hand = player.GetHand();
        List<string> handCards = hand.Select(card => card.ToString()).ToList();
    
        string playerName = player.Superstar.Name;
        int selectedCardIndex = _view.AskPlayerToSelectACardToDiscard(handCards, 
            playerName, 
            playerName, 
            1);
    
        Card selectedCard = hand[selectedCardIndex];
        hand.Remove(selectedCard);
        player.GetRingside().Add(selectedCard);
    }

    private void OpponentDiscardCard(Player opponent)
    {
        List<Card> hand = opponent.GetHand();
        List<string> handCards = hand.Select(card => card.ToString()).ToList();
    
        string opponentName = opponent.Superstar.Name;
        int selectedCardIndex = _view.AskPlayerToSelectACardToDiscard(handCards, 
            opponentName, 
            opponentName, 
            1);
    
        Card selectedCard = hand[selectedCardIndex];
        hand.Remove(selectedCard);
        opponent.GetRingside().Add(selectedCard);
    }

}