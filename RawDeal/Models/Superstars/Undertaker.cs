using RawDealView;
namespace RawDeal.Models.Superstars;

public class Undertaker : Superstar
{
    private readonly View _view;

    public Undertaker(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.InMenu;
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
        DiscardTwoCards(player);
        RecoverCardFromRingsideToHand(player);
        MarkAbilityAsUsed();
    }

    public override bool CanUseAbility(Player player)
    {
        List<Card> hand = player.GetHand();
        return !HasUsedAbility && hand.Count >= 2;
    }

    private void DiscardTwoCards(Player player)
    {
        string playerName = player.Superstar.Name;
        List<Card> hand = player.GetHand();
        List<Card> ringside = player.GetRingside();

        for (int i = 0; i < 2; i++)
        {
            List<string> handCards = hand.Select(card => card.ToString()).ToList();
            int selectedCardIndex = _view.AskPlayerToSelectACardToDiscard(handCards, 
                playerName, 
                playerName, 
                2 - i);
            Card selectedCard = hand[selectedCardIndex];
        
            hand.Remove(selectedCard);
            ringside.Add(selectedCard);
        }
    }

    private void RecoverCardFromRingsideToHand(Player player)
    {
        string playerName = player.Superstar.Name;
        List<Card> ringside = player.GetRingside();
        List<string> ringsideCards = ringside.Select(card => card.ToString()).ToList();
    
        int selectedCardIndex = _view.AskPlayerToSelectCardsToPutInHisHand(playerName, 1, ringsideCards);
        Card selectedCard = ringside[selectedCardIndex];
    
        ringside.Remove(selectedCard);
        player.GetHand().Add(selectedCard);
    }

}