namespace RawDeal.Models.Superstars;
public class SuperstarData
{
    public string Name { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public int HandSize { get; set; } = 0;
    public int SuperstarValue { get; set; } = 0;
    public string SuperstarAbility { get; set; } = string.Empty;
    public Card SuperstarCard { get; set; } = new Card();
}