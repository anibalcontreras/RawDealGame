namespace RawDeal.Models
{
    public class Deck
    {
        public Deck()
        {
            Superstar = new Superstar();
            Cards = new List<Card>();
        }
        public Superstar Superstar { get; set; }
        public List<Card> Cards { get; set; }

        public override string ToString()
        {
            return $"Superstar: {Superstar.Name}\n" +
                    $"Cards: {Cards}\n";
        }
    }    
}
