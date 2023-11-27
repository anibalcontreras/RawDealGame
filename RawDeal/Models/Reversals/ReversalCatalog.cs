using RawDeal.Exceptions;
using RawDealView;
namespace RawDeal.Models.Reversals;

public class ReversalCatalog
{
    private readonly View _view;
    private Dictionary<string, Reversal> _reversals = new Dictionary<string, Reversal>();
    
    public bool TryGetReversalBy(string cardTitle, out Reversal reversal)
    {
        return _reversals.TryGetValue(cardTitle, out reversal);
    }

    public Reversal GetReversalBy(string cardTitle)
    {
        if (_reversals.TryGetValue(cardTitle, out Reversal reversal))
        {
            return reversal;
        }

        throw new ReversalNotFoundException();
    }


    public ReversalCatalog(View view)
    {
        _view = view;
        InitializeReversals();
    }

    private void InitializeReversals()
    {
        _reversals["Step Aside"] = new ReversalStrike(_view);
        _reversals["Escape Move"] = new ReversalGrapple(_view);
        _reversals["Break the Hold"] = new ReversalSubmission(_view);
        _reversals["No Chance in Hell"] = new ReversalAction(_view);
    }
}