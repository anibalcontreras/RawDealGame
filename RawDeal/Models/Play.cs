using RawDealView.Formatters;

namespace RawDeal.Models;

public class Play : IViewablePlayInfo
{
    private IViewableCardInfo _cardInfo;
    private string _playedAs;

    public Play(IViewableCardInfo cardInfo, string playedAs)
    {
        _cardInfo = cardInfo;
        _playedAs = playedAs;
    }

    public IViewableCardInfo CardInfo => _cardInfo;
    public string PlayedAs => _playedAs;

    public bool IsPlayable(int playerFortitude)
    {
        int cardFortitude = int.Parse(_cardInfo.Fortitude);

        return (_cardInfo.Types.Contains("Maneuver") || _cardInfo.Types.Contains("Action")) && cardFortitude <= playerFortitude;
    }

    public int GetCardDamageAsInt()
    {
        string damage = _cardInfo.Damage;
        return int.Parse(damage);
    }

    public static List<string> GetFormattedPlayableCards(List<Card> cards, int playerFortitude)
    {
        List<Play> playablePlays = GetPlayablePlays(cards, playerFortitude);
        return playablePlays.Select(Formatter.PlayToString).ToList();
    }

    public static List<Play> GetPlayablePlays(List<Card> cards, int playerFortitude)
    {
        List<Play> plays = ConvertCardsToPlays(cards);
        return plays.Where(play => play.IsPlayable(playerFortitude)).ToList();
    }

    private static List<Play> ConvertCardsToPlays(List<Card> cards)
    {
        return cards.Select(card => new Play(card, card.GetTypesAsString())).ToList();
    }


    public static List<Card> GetPlayableCards(List<Card> cards, int playerFortitude)
    {
        return cards.Where(card => new Play(card, card.GetTypesAsString()).IsPlayable(playerFortitude)).ToList();
    }

}