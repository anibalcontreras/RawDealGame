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
            UseAbility(player, opponent);
    }
    
    public override void UseAbility(Player player, Player opponent)
    {
        if (CanUseAbility(player) && WantsToUseAbility(player))
        {
            RecoverCardFromRingsideToArsenal(player);
            MarkAbilityAsUsed();
        }
    }

    private bool CanUseAbility(Player player)
    {
        return player.GetRingside().Count > 0;
    }

    private bool WantsToUseAbility(Player player)
    {
        Console.Write("ACÁ BUSCANDO SI QUIERE ACTIVAR SU HABILIDAD");
        bool wantsToUseAbility = _view.DoesPlayerWantToUseHisAbility(player.Superstar.Name);
        if (wantsToUseAbility)
        {
            _view.SayThatPlayerIsGoingToUseHisAbility(player.Superstar.Name, player.Superstar.SuperstarAbility);
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
        List<string> ringsideCards = player.GetRingside().Select(card => card.ToString()).ToList();
        return _view.AskPlayerToSelectCardsToRecover(player.Superstar.Name, 1, ringsideCards);
    }

    private void MoveSelectedCardToArsenalBottom(Player player, int selectedCardIndex)
    {
        Card selectedCard = player.GetRingside()[selectedCardIndex];
        player.GetRingside().Remove(selectedCard);
        player.GetArsenal().Insert(0, selectedCard);
    }
}