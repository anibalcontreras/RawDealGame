namespace RawDeal.Models
{
    public class Card
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
        public Superstar()
        {
            Name = string.Empty;
            Logo = string.Empty;
            HandSize = 0;
            SuperstarValue = 0;
            SuperstarAbility = string.Empty;
            SuperstarCard = new Card();
        }

        public string Name { get; set; }
        public string Logo { get; set; }
        public int HandSize { get; set; }
        public int SuperstarValue { get; set; }
        public string SuperstarAbility { get; set; }
        public Card SuperstarCard { get; set; }

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
