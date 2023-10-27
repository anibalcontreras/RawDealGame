using RawDealView;
namespace RawDeal.Models.Effects;

public class EffectCatalog
{
    private readonly View _view;
    private Dictionary<string, Effect> _effects = new Dictionary<string, Effect>();
    public Effect GetEffectBy(string cardTitle, string playType)
    {
        if (IsHybridCard(cardTitle) && playType == "MANEUVER")
        {
            return new NoEffect(_view);
        }
    
        if (_effects.ContainsKey(cardTitle))
            return _effects[cardTitle];
    
        return new NoEffect(_view);
    }

    public EffectCatalog(View view)
    {
        _view = view;
        InitializeEffects();
    }

    private void InitializeEffects()
    {
        _effects["Chop"] = new DrawCardEffect(_view);
        _effects["Arm Bar Takedown"] = new DrawCardEffect(_view);
        _effects["Collar & Elbow Lockup"] = new DrawCardEffect(_view);
    }
    private bool IsHybridCard(string cardTitle)
    {
        List<string> hybridCards = new List<string> { "Chop", "Arm Bar Takedown", "Collar & Elbow Lockup" };
        return hybridCards.Contains(cardTitle);
    }
}