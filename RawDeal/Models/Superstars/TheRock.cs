using RawDealView;
namespace RawDeal.Models.Superstars;

public class TheRock : Superstar
{
    private readonly View _view;

    public TheRock(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.StartOfTurn;
    }

    public override void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
        if (activationTime == ActivationMoment && !HasUsedAbility)
            UseAbility(player);
    }

    private void UseAbility(Player player)
    {
        if (CanUseAbility(player) && WantsToUseAbility(player))
        {
            RecoverCardFromRingsideToArsenal(player);
            MarkAbilityAsUsed();
        }
    }

    private bool CanUseAbility(Player player)
    {
        List<Card> ringside = player.GetRingside();
        return ringside.Count > 0;
    }

    private bool WantsToUseAbility(Player player)
    {
        string playerName = player.Superstar.Name;
        bool wantsToUseAbility = _view.DoesPlayerWantToUseHisAbility(playerName);
    
        if (wantsToUseAbility)
        {
            string superstarAbility = player.Superstar.SuperstarAbility;
            _view.SayThatPlayerIsGoingToUseHisAbility(playerName, superstarAbility);
            return true;
        }
        return false;
    }

    private void RecoverCardFromRingsideToArsenal(Player player)
    {
        int selectedCardIndex = GetSelectedCardIndexFromRingside(player);
        MoveSelectedCardToArsenalBottom(player, selectedCardIndex);
    }

    private int GetSelectedCardIndexFromRingside(Player player)
    {
        List<Card> ringside = player.GetRingside();
        List<string> ringsideCards = ringside.Select(card => card.ToString()).ToList();
        string playerName = player.Superstar.Name;
    
        return _view.AskPlayerToSelectCardsToRecover(playerName, 1, ringsideCards);
    }

    private void MoveSelectedCardToArsenalBottom(Player player, int selectedCardIndex)
    {
        List<Card> ringside = player.GetRingside();
        Card selectedCard = ringside[selectedCardIndex];
    
        ringside.Remove(selectedCard);
    
        List<Card> arsenal = player.GetArsenal();
        arsenal.Insert(0, selectedCard);
    }

}