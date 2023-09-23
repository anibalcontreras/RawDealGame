using RawDealView.Formatters;

namespace RawDeal.Models;

public class Play : IViewablePlayInfo
{
    private IViewableCardInfo _cardInfo;
    private string _playedAs;

    private Play(IViewableCardInfo cardInfo, string playedAs)
    {
        _cardInfo = cardInfo;
        _playedAs = playedAs;
    }

    public IViewableCardInfo CardInfo => _cardInfo;
    public string PlayedAs => _playedAs;

    private bool IsPlayable(int playerFortitude) =>
        (IsManeuverOrAction && CardFortitude <= playerFortitude);

    private bool IsManeuverOrAction => 
        _cardInfo.Types.Contains("Maneuver") || _cardInfo.Types.Contains("Action");

    private int CardFortitude => int.Parse(_cardInfo.Fortitude);

    public int GetCardDamageAsInt()
    {
        string damage = _cardInfo.Damage;
        return int.Parse(damage);
    }
    public static List<string> GetFormattedPlayableCards(List<Card> cards, int playerFortitude) =>
        GetPlayablePlays(cards, playerFortitude).Select(Formatter.PlayToString).ToList();

    public static List<Play> GetPlayablePlays(List<Card> cards, int playerFortitude) =>
        ConvertCardsToPlays(cards).Where(play => play.IsPlayable(playerFortitude)).ToList();

    private static List<Play> ConvertCardsToPlays(List<Card> cards) =>
        cards.Select(card => new Play(card, card.GetTypesAsString())).ToList();

    public static List<Card> GetPlayableCards(List<Card> cards, int playerFortitude) =>
        cards.Where(card => new Play(card, card.GetTypesAsString()).IsPlayable(playerFortitude)).ToList();
}