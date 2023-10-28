using RawDealView.Formatters;
namespace RawDeal.Models;
public class Card : IViewableCardInfo
{
    public string Title { get; set; } = string.Empty;
    public List<string> Types { get; set; } = new List<string>();
    public List<string> Subtypes { get; set; } = new List<string>();
    public string Fortitude { get; set; } = string.Empty;
    public string Damage { get; set; } = string.Empty;
    public string StunValue { get; set; } = string.Empty;
    public string CardEffect { get; set; } = string.Empty;
    
    public string GetTypesAsString()
    {
        return string.Join(", ", Types.Select(type => type.ToUpper()));
    }
    public override string ToString()
    {
        return Formatter.CardToString(this);
    }
    public virtual Card Clone()
    {
        return new Card
        {
            Title = Title,
            Types = new List<string>(Types),
            Subtypes = new List<string>(Subtypes),
            Fortitude = Fortitude,
            Damage = Damage,
            StunValue = StunValue,
            CardEffect = CardEffect
        };
    }
    
    public enum CardType
    {
        MANEUVER,
        ACTION,
        REVERSAL,
    }
}
