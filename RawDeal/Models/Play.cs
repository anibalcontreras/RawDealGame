using RawDealView.Formatters;

namespace RawDeal.Models;

public class Play : IViewablePlayInfo
{
    private readonly IViewableCardInfo _cardInfo;
    private readonly string _playedAs;

    public Play(IViewableCardInfo cardInfo, string playedAs)
    {
        _cardInfo = cardInfo;
        _playedAs = playedAs;
    }

    public IViewableCardInfo CardInfo => _cardInfo;
    public string PlayedAs => _playedAs;
    
    private bool IsPlayable(int playerFortitude) => IsManeuverOrAction && CardFortitude <= playerFortitude;
    private bool IsManeuverOrAction => _cardInfo.Types.Contains("Maneuver") || _cardInfo.Types.Contains("Action");
    private int CardFortitude => int.Parse(_cardInfo.Fortitude);

    public static List<string> GetFormattedPlayableCards(List<Card> cards, int playerFortitude) =>
        GetPlayablePlays(cards, playerFortitude).Select(Formatter.PlayToString).ToList();

    public static List<Play> GetPlayablePlays(List<Card> cards, int playerFortitude) =>
        ConvertCardsToPlays(cards).Where(play => play.IsPlayable(playerFortitude)).ToList();

    private static List<Play> ConvertCardsToPlays(List<Card> cards) =>
        cards.SelectMany(DivideCardByTypes).ToList();

    public static List<Card> GetPlayableCards(List<Card> cards, int playerFortitude) =>
        cards.Where(card => GetPlayablePlays(new List<Card> { card }, playerFortitude).Any()).ToList();
    
    private static List<Play> DivideCardByTypes(Card card)
    {
        List<Play> dividedPlays = new List<Play>();
        if (card.Types.Contains("Maneuver") && card.Types.Contains("Action"))
        {
            dividedPlays.Add(new Play(card, "ACTION"));
            dividedPlays.Add(new Play(card, "MANEUVER"));
        }
        else
            dividedPlays.Add(new Play(card, card.GetTypesAsString().ToUpper()));
        return dividedPlays;
    }
}
