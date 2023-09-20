namespace RawDeal.Models;
public class Deck
{
    public Superstar Superstar { get; private set; }
    public List<Card> Cards { get; private set; }

    public Deck()
    {
        Cards = new List<Card>();
    }

    public void AddSuperstar(Superstar superstar)
    {
        if (Superstar != null)
            throw new InvalidOperationException("El deck ya tiene un Superstar asignado.");

        Superstar = superstar ?? throw new ArgumentNullException(nameof(superstar));
    }

    public void AddCard(Card card)
    {
        Cards.Add(card ?? throw new ArgumentNullException(nameof(card)));
    }
}