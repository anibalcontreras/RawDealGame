using RawDeal.Models;
using RawDealView;
namespace RawDeal.Controllers;

public class PlayerActionsController
{
    private readonly View _view;

    public PlayerActionsController(View view)
    {
        _view = view;
    }

    public void DrawCard(Player player)
    {
        if (!player.GetArsenal().Any()) return;
        Card lastCard = player.GetArsenal().Last();
        player.GetArsenal().Remove(lastCard);
        player.GetHand().Add(lastCard);
    }

    public bool ReceiveDamage(Player player, int damageAmount)
    {
        for (int i = 0; i < damageAmount; i++)
        {
            if (!player.GetArsenal().Any()) return true;
            Card lastCard = player.GetArsenal().Last();
            player.GetArsenal().Remove(lastCard);
            player.GetRingside().Add(lastCard);
            _view.ShowCardOverturnByTakingDamage(lastCard.ToString(), i + 1, damageAmount);
            Console.WriteLine(lastCard.GetType());
        }
        return false;
    }

    public void ApplyDamage(Player player, int cardIndex)
    {
        Card cardToApply = player.GetHand()[cardIndex];
        player.GetHand().RemoveAt(cardIndex);
        player.GetRingArea().Add(cardToApply);
    }
    public void DiscardCard(Player player, Card cardToDiscard)
    {
        if (player.GetHand().Any(card => card.Title == cardToDiscard.Title))
        {
            AddCardToRingside(player, cardToDiscard);
            RemoveCardFromHand(player, cardToDiscard);
            _view.SayThatPlayerMustDiscardThisCard(player.Superstar.Name, cardToDiscard.Title);
        }
    }
    private void AddCardToRingside(Player player, Card card)
    {
        player.GetRingside().Add(card);
    }

    private void RemoveCardFromHand(Player player, Card card)
    {
        player.GetHand().Remove(card);
    }
}