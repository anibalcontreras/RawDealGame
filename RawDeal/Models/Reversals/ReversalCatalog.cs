using RawDealView;
using RawDeal.Logic;
using RawDeal.Models.Effects; // Asegúrate de agregar este using para acceder a DeckLoader


namespace RawDeal.Models.Reversals;
public class ReversalCatalog
{
    private readonly View _view;
    private Dictionary<string, Reversal> _reversals = new Dictionary<string, Reversal>();

    public Reversal GetReversalBy(string cardTitle)
    {
        if (_reversals.ContainsKey(cardTitle))
            return _reversals[cardTitle];
        
        Card card = DeckLoader.GetCardByName(cardTitle);
        if (card.Types.Contains("Reversal"))
        {
            return new NoEffectReversal(_view, card);
        }

        return null;
    }
    // public Reversal GetReversalBy(string cardTitle)
    // {
    //     if (_reversals.ContainsKey(cardTitle))
    //         return _reversals[cardTitle];
    //
    //     Card card = DeckLoader.GetCardByName(cardTitle);
    //     if (card.Types.Contains("Reversal"))
    //     {
    //         return new NoEffectReversal(_view, card);
    //     }
    //
    //     throw new ReversalException("The Card is not a reversal");
    // }

    public ReversalCatalog()
    {
        // _view = view;
        InitializeReversals();
    }

    private void InitializeReversals()
    {
        _reversals["Step Aside"] = GetReversalBy("Step Aside");
        _reversals["Escape Move"] = GetReversalBy("Escape Move");
        _reversals["Break the Hold"] = GetReversalBy("Break the Hold");
        _reversals["No Chance in Hell"] = GetReversalBy("No Chance in Hell");
    }
}