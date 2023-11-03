using RawDealView;
namespace RawDeal.Models.Superstars;
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