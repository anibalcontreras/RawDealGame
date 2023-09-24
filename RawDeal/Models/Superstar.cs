namespace RawDeal.Models;

public class SuperstarData
{
    public string Name { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public int HandSize { get; set; } = 0;
    public int SuperstarValue { get; set; } = 0;
    public string SuperstarAbility { get; set; } = string.Empty;
    public Card SuperstarCard { get; set; } = new Card();
}
public abstract class Superstar
{
    public string Name { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public int HandSize { get; set; } = 0;
    public int SuperstarValue { get; set; } = 0;
    public string SuperstarAbility { get; set; } = string.Empty;
    public Card SuperstarCard { get; set; } = new Card();
    public bool HasUsedAbility { get; private set; } = false;
    public virtual void UseAbility(Player player, Player opponent)
    {
        HasUsedAbility = true;
    }
    public void ResetAbilityUsage()
    {
        HasUsedAbility = false;
    }
    public AbilityActivation ActivationMoment { get; set; } = AbilityActivation.None;
}

public class HHH : Superstar
{
    public HHH()
    {
        ActivationMoment = AbilityActivation.None;
    }

    public override void UseAbility(Player player, Player opponent)
    {
    }
}

public class StoneCold : Superstar
{
    public StoneCold()
    {
    }

    public override void UseAbility(Player player, Player opponent)
    {
    }
}

public class Undertaker : Superstar
{
    public Undertaker()
    {
    }

    public override void UseAbility(Player player, Player opponent)
    {
    }
}

public class Mankind : Superstar
{
    public Mankind()
    {
    }

    public override void UseAbility(Player player, Player opponent)
    {
    }
}

public class TheRock : Superstar
{
    public TheRock()
    {
    }

    public override void UseAbility(Player player, Player opponent)
    {
    }
}

public class Kane : Superstar
{
    public Kane()
    {
        ActivationMoment = AbilityActivation.StartOfTurn;
    }

    public override void UseAbility(Player player, Player opponent)
    {
        base.UseAbility(player, opponent);
        opponent.ReceiveDamage(1);
    }
}

public class Jericho : Superstar
{
    public Jericho()
    {
    }

    public override void UseAbility(Player player, Player opponent)
    {
    }
}

public static class SuperstarFactory
{
    public static Superstar CreateSuperstar(string logo)
    {
        switch (logo)
        {
            case "HHH":
                return new HHH();
            case "StoneCold":
                return new StoneCold();
            case "Undertaker":
                return new Undertaker();
            case "Mankind":
                return new Mankind();
            case "TheRock":
                return new TheRock();
            case "Kane":
                return new Kane();
            case "Jericho":
                return new Jericho();
            default:
                throw new ArgumentException($"No superstar found with the logo {logo}");
        }
    }
}

public enum AbilityActivation
{
    StartOfTurn,
    DrawSegment,
    None
}