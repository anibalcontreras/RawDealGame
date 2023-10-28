using RawDealView;
using RawDeal.Models.Superstars;
namespace RawDeal.Models;
public abstract class Superstar
{
    public string Name { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public int HandSize { get; set; } = 0;
    public int SuperstarValue { get; set; } = 0;
    public string SuperstarAbility { get; set; } = string.Empty;
    public Card SuperstarCard { get; set; } = new Card();
    public bool HasUsedAbility { get; set;  } = false;
    public AbilityActivation ActivationMoment { get; set; } = AbilityActivation.None;
    
    public virtual void ActivateAbility(Player player, Player opponent, AbilityActivation activationTime)
    {
    }
    public virtual bool CanUseAbility(Player player)
    {
        return true;
    }
    protected virtual void MarkAbilityAsUsed()
    {
        HasUsedAbility = true;
    }
    public void MarkAbilityAsUnused()
    {
        HasUsedAbility = false;
    }
    
    public virtual int CalculateDamage(int originalDamage)
    {
        return originalDamage;
    }

}
public static class SuperstarFactory
{
    public static Superstar CreateSuperstar(string logo, View view)
    {
        switch (logo)
        {
            case "HHH":
                return new HHH(view);
            case "StoneCold":
                return new StoneCold(view);
            case "Undertaker":
                return new Undertaker(view);
            case "Mankind":
                return new Mankind(view);
            case "TheRock":
                return new TheRock(view);
            case "Kane":
                return new Kane(view);
            case "Jericho":
                return new Jericho(view);
            default:
                throw new ArgumentException($"No se encontró el siguiente Superstar: {logo}");
        }
    }
}
public class SuperstarData
{
    public string Name { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public int HandSize { get; set; } = 0;
    public int SuperstarValue { get; set; } = 0;
    public string SuperstarAbility { get; set; } = string.Empty;
    public Card SuperstarCard { get; set; } = new Card();
}
public enum AbilityActivation
{
    StartOfTurn,
    InMenu,
    Automatic,
    None
}