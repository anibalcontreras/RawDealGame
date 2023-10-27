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
}
