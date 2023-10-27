using RawDealView;
namespace RawDeal.Models.Reversals;

public abstract class Reversal
{
    protected readonly View _view;

    protected readonly Card _card;
    public Reversal(View view, Card card)
    {
        _view = view;
        _card = card;
    }
    public int GetStunValue()
    {
        return int.Parse(_card.StunValue);
    }
    
    public string GetTypesAsString()
    {
        return string.Join(", ", _card.Types.Select(type => type.ToUpper()));
    }
    
    public string GetSubtypesAsString()
    {
        return string.Join(", ", _card.Subtypes.Select(type => type.ToUpper()));
    }

    public override string ToString()
    {
        return _card.Title;
    }

    public abstract bool CanReverseFromDeck(Card playedCard);

    public abstract bool CanReverseFromHand(Card playedCard, Player player);
    
}