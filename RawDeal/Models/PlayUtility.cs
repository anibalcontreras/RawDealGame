using RawDealView.Formatters;

namespace RawDeal.Models;
public static class PlayUtility
{
    public static List<string> GetFormattedPlayableCards(List<Card> cards, int playerFortitude) =>
        GetPlayablePlays(cards, playerFortitude).Select(Formatter.PlayToString).ToList();

    public static List<Play> GetPlayablePlays(List<Card> cards, int playerFortitude) =>
        ConvertCardsToPlays(cards).Where(play => IsPlayable(play, playerFortitude)).ToList();

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

    private static List<Play> ConvertCardsToPlays(List<Card> cards) =>
        cards.SelectMany(DivideCardByTypes).ToList();
    
    public static List<Card> GetPlayableCards(List<Card> cards, int playerFortitude) =>
        cards.Where(card => GetPlayablePlays(new List<Card> { card }, playerFortitude).Any()).ToList();

    private static List<Play> DivideCardByTypes(Card card)
    {
        List<Play> dividedPlays = new List<Play>();

        foreach (var type in card.Types)
        {
            dividedPlays.Add(new Play(card, type.ToUpper()));
        }
        return dividedPlays;
    }
}