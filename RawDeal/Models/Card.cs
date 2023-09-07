using RawDealView.Formatters;

namespace RawDeal.Models
{
    public class Card : IViewableCardInfo
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Types { get; set; } = new List<string>();
        public List<string> Subtypes { get; set; } = new List<string>();

        public string Fortitude { get; set; } = string.Empty;
        public string Damage { get; set; } = string.Empty;
        public string StunValue { get; set; } = string.Empty;
        public string CardEffect { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Title: {Title}\n" +
                    $"Types: {string.Join(", ", Types)}\n" +
                    $"Subtypes: {string.Join(", ", Subtypes)}\n" +
                    $"Fortitude: {Fortitude}\n" +
                    $"Damage: {Damage}\n" +
                    $"StunValue: {StunValue}\n" +
                    $"CardEffect: {CardEffect}\n";
        }

    }

    public class Superstar
    {
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public int HandSize { get; set; } = 0;
        public int SuperstarValue { get; set; } = 0;
        public string SuperstarAbility { get; set; } = string.Empty;
        public Card SuperstarCard { get; set; } = new Card();

        public override string ToString()
        {
            return $"Name: {Name}\n" +
                    $"Logo: {Logo}\n" +
                    $"HandSize: {HandSize}\n" +
                    $"SuperstarValue: {SuperstarValue}\n" +
                    $"SuperstarAbility: {SuperstarAbility}";
        }
    }
}
