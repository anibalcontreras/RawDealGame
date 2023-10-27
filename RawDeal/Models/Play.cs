using RawDeal.Exceptions;
using RawDeal.Models.Reversals;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Models;

public class Play : IViewablePlayInfo
{
    private readonly IViewableCardInfo _cardInfo;
    private readonly string _playedAs;

    private Play(IViewableCardInfo cardInfo, string playedAs)
    {
        _cardInfo = cardInfo;
        _playedAs = playedAs;
    }

    public IViewableCardInfo CardInfo => _cardInfo;
    public string PlayedAs => _playedAs;
    private static ReversalCatalog _reversalCatalog;

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

    public static List<Card> GetPlayableReversals(List<Card> cards, Card playedCard, Player player) =>
        cards.Where(card => CanReverseFromHand(card, playedCard, player)).ToList();

    public static bool CanReverseFromHand(Card reversalCard, Card playedCard, Player player)
    {
        Console.WriteLine("Llegamos a CanReverseFromHand");
        if (_reversalCatalog == null)
        {
            return false;
        }
        Reversal potentialReversal = _reversalCatalog.GetReversalBy(reversalCard.Title);
        Console.WriteLine("El potential reversal es " + potentialReversal.ToString());
        return potentialReversal.CanReverseFromHand(playedCard, player);
    }
    

    public static List<string> GetFormattedPlayableReversals(List<Card> cards, Card playedCard, Player player)
    {
        List<Play> playableReversalPlays = ConvertCardsToReversalPlays(GetPlayableReversals(cards, playedCard, player));
        return playableReversalPlays.Select(Formatter.PlayToString).ToList();
    }
    
    public static List<Play> ConvertCardsToReversalPlays(List<Card> cards) =>
        cards.SelectMany(DivideCardByTypes).ToList();
    
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
