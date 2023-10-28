namespace RawDeal.Models.Superstars;

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
    
    protected void MarkAbilityAsUsed()
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
