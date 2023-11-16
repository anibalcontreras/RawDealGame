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
        throw new KeyNotFoundException($"No reversal found for card title: {cardTitle}");
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
        _reversals["Rolling Takedown"] = new ReversalGrappleSpecial(_view);
        _reversals["Knee to the Gut"] = new ReversalStrikeSpecial(_view);
        _reversals["Elbow to the Face"] = new ReversalSpecial(_view);
        _reversals["Manager Interferes"] = new Heel(_view);
        _reversals["Chyna Interferes"] = new Unique(_view);
        _reversals["Clean Break"] = new Face(_view);
        _reversals["Jockeying for Position"] = new SetUp(_view);
    }
}