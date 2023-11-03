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
        List<Card> arsenal = player.GetArsenal();
        List<Card> hand = player.GetHand();

        if (!arsenal.Any()) return;

        Card lastCard = arsenal.Last();
        arsenal.Remove(lastCard);
        hand.Add(lastCard);
    }

    public bool ReceiveDamage(Player player, int damageAmount)
    {
        List<Card> arsenal = player.GetArsenal();
        List<Card> ringside = player.GetRingside();

        for (int i = 0; i < damageAmount; i++)
        {
            if (!arsenal.Any()) return true;

            Card lastCard = arsenal.Last();
            arsenal.Remove(lastCard);
            ringside.Add(lastCard);
            _view.ShowCardOverturnByTakingDamage(lastCard.ToString(), i + 1, damageAmount);
        }
        return false;
    }

    public void ApplyDamage(Player player, int cardIndex)
    {
        List<Card> hand = player.GetHand();
        List<Card> ringArea = player.GetRingArea();

        Card cardToApply = hand[cardIndex];
        hand.RemoveAt(cardIndex);
        ringArea.Add(cardToApply);
    }

    public void DiscardCard(Player player, Card cardToDiscard)
    {
        List<Card> hand = player.GetHand();

        if (hand.Any(card => card.Title == cardToDiscard.Title))
        {
            AddCardToRingside(player, cardToDiscard);
            RemoveCardFromHand(player, cardToDiscard);
            _view.SayThatPlayerMustDiscardThisCard(player.Superstar.Name, cardToDiscard.Title);
        }
    }

    private void AddCardToRingside(Player player, Card card)
    {
        List<Card> ringside = player.GetRingside();
        ringside.Add(card);
    }

    private void RemoveCardFromHand(Player player, Card card)
    {
        List<Card> hand = player.GetHand();
        hand.Remove(card);
    }
}