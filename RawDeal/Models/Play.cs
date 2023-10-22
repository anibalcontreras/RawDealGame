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
        IsManeuverOrAction && CardFortitude <= playerFortitude;
    private bool IsManeuverOrAction => 
        _cardInfo.Types.Contains("Maneuver") || _cardInfo.Types.Contains("Action");
    private int CardFortitude => int.Parse(_cardInfo.Fortitude);
    public int GetCardDamageAsInt()
    {
        string damage = _cardInfo.Damage;
        return int.Parse(damage);
    }
    public static List<Play> DivideCardByTypes(Card card)
    {
        List<Play> dividedPlays = new List<Play>();

        if (card.Types.Contains("Maneuver") && card.Types.Contains("Action"))
        {
            // Si la carta tiene ambos tipos, crea dos instancias de Play
            Play playAction = new Play(card, "ACTION");
            Play playManeuver = new Play(card, "MANEUVER");
            dividedPlays.Add(playAction);
            dividedPlays.Add(playManeuver);
        }
        else
        {
            // Si la carta tiene un solo tipo, crea una instancia de Play
            var play = new Play(card, card.GetTypesAsString().ToUpper());
            dividedPlays.Add(play);
        }

        return dividedPlays;
    }

    // Métodos para obtener cartas formateadas y jugables
    public static List<string> GetFormattedPlayableCards(List<Card> cards, int playerFortitude) =>
        GetPlayablePlays(cards, playerFortitude)
            .Select(play => Formatter.PlayToString(play))
            .ToList();

    public static List<Play> GetPlayablePlays(List<Card> cards, int playerFortitude) =>
        ConvertCardsToPlays(cards)
            .Where(play => play.IsPlayable(playerFortitude))
            .ToList();

    private static List<Play> ConvertCardsToPlays(List<Card> cards) =>
        cards.SelectMany(card => DivideCardByTypes(card))
            .ToList();

    public static List<Card> GetPlayableCards(List<Card> cards, int playerFortitude) =>
        cards
            .Where(card => GetPlayablePlays(new List<Card> { card }, playerFortitude).Count > 0)
            .ToList();
}
