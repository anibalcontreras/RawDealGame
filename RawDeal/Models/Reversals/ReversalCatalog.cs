using RawDealView;
namespace RawDeal.Models.Reversals;

public class ReversalCatalog
{
    private readonly View _view;
    private Dictionary<string, Reversal> _reversals = new Dictionary<string, Reversal>();

    public Reversal GetReversalBy(string cardTitle)
    {
        if (_reversals.ContainsKey(cardTitle))
            return _reversals[cardTitle];

        return new NoEffectReversal(_view);
    }

    public ReversalCatalog(View view)
    {
        _view = view;
        InitializeReversals();
    }

    private void InitializeReversals()
    {
        // Aquí puedes agregar los reversals específicos
        _reversals["Step Aside"] = new NoEffectReversal(_view);
        _reversals["Escape Move"] = new NoEffectReversal(_view);
        _reversals["Break the Hold"] = new NoEffectReversal(_view);
        _reversals["No Chance in Hell"] = new NoEffectReversal(_view);
    }
}