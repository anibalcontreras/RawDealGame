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
    
    public string OriginalDamage { get; private set; }
    public override string ToString()
    {
        return Formatter.CardToString(this);
    }
    public Card Clone()
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
    
    public string PlayedAs { get; set; } = string.Empty;
    
    public void SetPlayedAs(string playedAs)
    {
        PlayedAs = playedAs;
    }
    
    public void TemporarilySetDamage(string damage)
    {
        Damage = damage;
    }
    
    public void IncrementDamage(int damage, int damageToAdd)
    {
        int.TryParse(Damage, out damage);
        damage += damageToAdd;
        Damage = damage.ToString();
    }
    
    public void DecrementDamage(int damage, int damageToSubtract)
    {
        int.TryParse(Damage, out damage);
        damage -= damageToSubtract;
        Damage = damage.ToString();
    }
    public void ResetDamageToOriginal()
    {
        Damage = OriginalDamage;
    }
    
    public enum CardType
    {
        Maneuver,
        Action,
    }
}
