using RawDeal.Models.Reversals;
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

    // public int GetCardDamageAsInt() => int.Parse(_cardInfo.Damage);

    public static List<string> GetFormattedPlayableCards(List<Card> cards, int playerFortitude) =>
        GetPlayablePlays(cards, playerFortitude).Select(Formatter.PlayToString).ToList();

    public static List<Play> GetPlayablePlays(List<Card> cards, int playerFortitude) =>
        ConvertCardsToPlays(cards).Where(play => play.IsPlayable(playerFortitude)).ToList();

    private static List<Play> ConvertCardsToPlays(List<Card> cards) =>
        cards.SelectMany(DivideCardByTypes).ToList();

    public static List<Card> GetPlayableCards(List<Card> cards, int playerFortitude) =>
        cards.Where(card => GetPlayablePlays(new List<Card> { card }, playerFortitude).Any()).ToList();

    public static List<string> GetFormattedPlayableReversals(List<Card> opponentHand, Card playedCard, int opponentFortitude, string playedAs) =>
        GetPlayablePlaysReversals(opponentHand, playedCard, opponentFortitude, playedAs).Select(Formatter.PlayToString).ToList();

    public static List<Play> GetPlayablePlaysReversals(List<Card> opponentHand, Card playedCard, int opponentFortitude, string playedAs)
    {
        return GetPlayableReversals(opponentHand, playedCard, opponentFortitude, playedAs).Select(card => new Play(card, "REVERSAL")).ToList();
    }
    
    // private static List<Card> GetPlayableReversals(List<Card> opponentHand, Card playedCard, Player opponent)
    // {
    //     return opponentHand.Where(reversalCard => 
    //     {
    //         Reversal reversal = _reversalCatalog.GetReversalBy(reversalCard.Title);
    //         return reversal != null && reversal.CanReverse(playedCard, opponent);
    //     }).ToList();
    // }
    private static List<Card> GetPlayableReversals(List<Card> opponentHand, Card playedCard, int opponentFortitude, string playedAs) =>
        opponentHand.Where(reversalCard => CanReverse(playedCard, reversalCard, opponentFortitude, playedAs)).ToList();
    private static bool CanReverse(Card playedCard, Card reversalCard, int opponentFortitude, string playedAs)
    {
        bool canReverse = HasSufficientFortitude(reversalCard, opponentFortitude) &&
                          IsReversalCard(reversalCard) &&
                          CardPlayedAsIntendedType(playedCard, playedAs) &&
                          ReversalEffectMatchesPlayedCard(playedCard, reversalCard, playedAs);
        return canReverse;
    }
    
    private static bool IsReversalCard(Card reversalCard)
    {
        bool isReversal = reversalCard.Types.Contains("Reversal");
        return isReversal;
    }
    
    private static bool HasSufficientFortitude(Card reversalCard, int opponentFortitude)
    {
        bool hasFortitude = opponentFortitude >= int.Parse(reversalCard.Fortitude);
        return hasFortitude;
    }
    
    private static bool CardPlayedAsIntendedType(Card playedCard, string playedAs)
    {
        bool isPlayedAsIntended = playedCard.Types.Any(type => string.Equals(type, playedAs, StringComparison.OrdinalIgnoreCase));
        return isPlayedAsIntended;
    }
    
    
    private static bool ReversalEffectMatchesPlayedCard(Card playedCard, Card reversalCard, string playedAs)
    {
        string playedCardTypeFromReversalSubtype = GetReversalTypeFromSubtype(reversalCard.Subtypes[0]);
        bool matches = playedCard.Subtypes.Any(subtype => String.Equals(subtype, playedCardTypeFromReversalSubtype, StringComparison.OrdinalIgnoreCase));
        
        if (String.Equals(playedAs, "ACTION", StringComparison.OrdinalIgnoreCase) && reversalCard.Subtypes.Contains("ReversalAction"))
        {
            matches = true;
        }
        
        else if (!String.Equals(playedAs, "ACTION", StringComparison.OrdinalIgnoreCase) && reversalCard.Subtypes.Contains("ReversalAction"))
        {
            matches = false;
        }
        
        return matches;
    }
    
    private static string GetReversalTypeFromSubtype(string subtype)
    {
        switch (subtype)
        {
            case "ReversalGrapple":
                return "Grapple";
            case "ReversalStrike":
                return "Strike";
            case "ReversalSubmission":
                return "Submission";
            case "ReversalAction":
                return "Action";
            default:
                return "";
        }
    }
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
