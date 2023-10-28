using RawDealView;
namespace RawDeal.Models.Superstars;
public class HHH : Superstar
{
    private readonly View _view;
    public HHH(View view)
    {
        _view = view;
        ActivationMoment = AbilityActivation.None;
    }
}