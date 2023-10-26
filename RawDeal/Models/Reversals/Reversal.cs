using RawDealView;
namespace RawDeal.Models.Reversals;

public abstract class Reversal
{
    protected readonly View _view;
    public string Title { get; set; }
    public List<string> Types { get; set; }
    public List<string> Subtypes { get; set; }
    public string Fortitude { get; set; }
    public string StunValue { get; set; }
    public Reversal(View view, Card card)
    {
        _view = view;
        Title = card.Title;
        Types = card.Types;
        Subtypes = card.Subtypes;
        Fortitude = card.Fortitude;
        StunValue = card.StunValue;
    }
    public abstract bool Apply(Player player, Player opponent, Card playedCard);
    
    public override string ToString()
    {
        return GetType().Name + ": Puede revertir los siguientes tipos: " + string.Join(", ", Subtypes);
    }
    public int GetStunValue()
    {
        return int.Parse(StunValue);
    }
    public bool CanReverse(Card playedCard, Player player)
    {
        return HasSufficientFortitude(player.Fortitude) &&
               IsReversalCard() &&
               CardPlayedAsIntendedType(playedCard) &&
               ReversalEffectMatchesPlayedCard(playedCard);
    }
    private bool IsReversalCard() => Types.Contains("Reversal");
    private bool HasSufficientFortitude(int playerFortitude) => playerFortitude >= int.Parse(Fortitude);
    private bool CardPlayedAsIntendedType(Card playedCard) => playedCard.Types.Any(type => Types.Contains(type));

    private bool ReversalEffectMatchesPlayedCard(Card playedCard)
    {
        return true;
    }
}