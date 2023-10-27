using RawDeal.Exceptions;

namespace RawDeal.Models.Reversals;

public class ReversalCatalog
{
    private Dictionary<string, Reversal> _reversals = new Dictionary<string, Reversal>();
    
    private static ReversalCatalog _instance;
    public static ReversalCatalog Instance => _instance ??= new ReversalCatalog();
    

    public Reversal GetReversalBy(string reversalName)
    {
        if (_reversals.TryGetValue(reversalName, out var reversal))
        {
            return reversal;
        }
        else
        {
            throw new ReversalException($"Reversal {reversalName} no encontrado.");
        }
    }
    
    public bool Contains(string reversalName)
    {
        return _reversals.ContainsKey(reversalName);
    }
    private ReversalCatalog()
    {
        InitializeReversals();
    }
    
    private void InitializeReversals()
    {
        _reversals["Step Aside"] = new NoEffectReversal();
        _reversals["Escape Move"] = new NoEffectReversal();
        _reversals["Break the Hold"] = new NoEffectReversal();
        _reversals["No Chance in Hell"] = new NoEffectReversal();
    }
}