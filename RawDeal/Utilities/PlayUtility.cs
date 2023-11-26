using RawDealView.Formatters;
using RawDeal.Models;
namespace RawDeal.Utilities;
public static class PlayUtility
{
    public static List<string> GetFormattedReversalCards(List<Card> cards)
    {
        List<Play> reversalPlays = ConvertCardsToPlaysForReversals(cards);
        return reversalPlays.Select(Formatter.PlayToString).ToList();
    }
    
    public static List<string> GetFormattedPlayableCards(List<Card> cards, int playerFortitude)
    {
        List<Play> playablePlays = GetPlayablePlays(cards, playerFortitude);
        return playablePlays.Select(Formatter.PlayToString).ToList();
    }

    public static List<Play> GetPlayablePlays(List<Card> cards, int playerFortitude)
    {
        return ConvertCardsToPlaysForPlays(cards).Where(play => IsPlayable(play, playerFortitude)).ToList();
    }
    

    private static bool IsPlayable(Play play, int playerFortitude)
    {
        bool cardTypeIsPlayable = CardTypeIsPlayable(play);
        bool playerHasSufficientFortitude = PlayerHasSufficientFortitude(play, playerFortitude);
        return cardTypeIsPlayable && playerHasSufficientFortitude;
    }

    private static bool CardTypeIsPlayable(Play play)
    {
        string cardTypeManeuver = Card.CardType.Maneuver.ToString().ToUpper();
        string cardTypeAction = Card.CardType.Action.ToString().ToUpper();
        List<string> playableTypes = new List<string> { cardTypeManeuver, cardTypeAction };
        return play.CardInfo.Types.Any(type => playableTypes.Contains(type.ToUpper()));
    }
    
    private static bool PlayerHasSufficientFortitude(Play play, int playerFortitude)
    {
        int cardFortitude = int.Parse(play.CardInfo.Fortitude);
        return cardFortitude <= playerFortitude;
    }

    private static List<Play> ConvertCardsToPlaysForPlays(List<Card> cards)
    {
        List<Play> plays = new List<Play>();
    
        foreach (var card in cards)
        {
            plays.AddRange(DivideCardByTypesForPlays(card));
        }
        return plays;
    }

    private static List<Play> ConvertCardsToPlaysForReversals(List<Card> cards)
    {
        List<Play> plays = new List<Play>();
    
        foreach (var card in cards)
        {
            plays.AddRange(DivideCardByTypesForReversals(card));
        }
        return plays;
    }

    public static List<Card> GetPlayableCards(List<Card> cards, int playerFortitude)
    {
        List<Card> playableCards = new List<Card>();
    
        foreach (var card in cards)
        {
            List<Card> singleCardList = new List<Card> { card };
            List<Play> playablePlaysForCard = GetPlayablePlays(singleCardList, playerFortitude);
            if (playablePlaysForCard.Any())
                playableCards.Add(card);
        }
        return playableCards;
    }
    private static List<Play> DivideCardByTypesForPlays(Card card)
    {
        List<Play> dividedPlays = new List<Play>();
        // Definir los tipos de cartas permitidos para ser divididos
        var allowedTypes = new HashSet<string> { "MANEUVER", "ACTION" };

        foreach (string type in card.Types)
        {
            // Convertir a mayúsculas para hacer la comprobación insensible a mayúsculas
            string upperType = type.ToUpper();
            // Comprobar si el tipo actual está en la lista de tipos permitidos
            if (allowedTypes.Contains(upperType))
            {
                Play newPlay = new Play(card, upperType);
                dividedPlays.Add(newPlay);
            }
        }
        return dividedPlays;
    }

    
    private static List<Play> DivideCardByTypesForReversals(Card card)
    {
        List<Play> dividedPlays = new List<Play>();

        foreach (string type in card.Types)
        {
            Play newPlay = new Play(card, type.ToUpper());
            dividedPlays.Add(newPlay);
        }
        return dividedPlays;
    }
}