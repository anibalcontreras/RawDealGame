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
}