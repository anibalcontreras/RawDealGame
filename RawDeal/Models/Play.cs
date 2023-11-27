using RawDealView.Formatters;

namespace RawDeal.Models;

public class Play : IViewablePlayInfo
{
    public Play(IViewableCardInfo cardInfo, string playedAs)
    {
        CardInfo = cardInfo;
        PlayedAs = playedAs;
    }
    public IViewableCardInfo CardInfo { get; }
    public string PlayedAs { get; }
}
