namespace RawDeal.Models;

public class Deck
{

    public Superstar Superstar { get; set; }
    public List<Card> Cards { get; set; } = new List<Card>();
    public Deck(Superstar superstar)
    {
        Superstar = superstar;
    }
}    

