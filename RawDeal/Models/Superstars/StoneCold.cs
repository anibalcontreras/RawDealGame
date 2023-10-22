using RawDealView;
namespace RawDeal.Models.Superstars;

public class StoneCold : Superstar
{
    private readonly View _view;
    
    public StoneCold(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.InMenu;
    }
    
    public override void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
        if (activationTime == ActivationMoment && !HasUsedAbility && player.GetArsenal().Count > 0)
        {
            UseAbility(player, opponent);
        }
    }
    
    public override void UseAbility(Player player, Player opponent)
    {
        AnnounceAbilityUsage(player);
        DrawCardFromArsenal(player);
        ReturnCardFromHandToArsenalBottom(player);
        MarkAbilityAsUsed();
    }

    private void AnnounceAbilityUsage(Player player)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.Superstar.Name, player.Superstar.SuperstarAbility);
    }

    private void DrawCardFromArsenal(Player player)
    {
        player.DrawCard();
        _view.SayThatPlayerDrawCards(player.Superstar.Name, 1);
    }

    private void ReturnCardFromHandToArsenalBottom(Player player)
    {
        List<string> formattedCardsInfo = player.GetFormattedCardsInfo(player.GetHand());
        int selectedCardIndex = _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(
            player.Superstar.Name, 
            formattedCardsInfo
        );
        Card selectedCard = player.GetHand()[selectedCardIndex];
        player.GetHand().Remove(selectedCard);
        player.GetArsenal().Insert(0, selectedCard);
    }
}