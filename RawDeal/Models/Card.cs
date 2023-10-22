using RawDealView.Formatters;
// using RawDeal.Models.Effects;
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
    // public int GetFortitude()
    // {
    //     return int.Parse(Fortitude);
    // }

    // public Card(string title)
    // {
    //     Title = title;
    // }
    public Card Clone()
    {
        return new Card
        {
            Title = Title,
            Types = Types,
            Subtypes = Subtypes,
            Fortitude = Fortitude,
            Damage = Damage,
            StunValue = StunValue,
            CardEffect = CardEffect
        };
    }
}
