using RawDeal.Models;

namespace RawDeal.Utilities;

public static class PlayerUtility
{
    public static bool HasValidSuperstar(this Player player)
    {
        return player?.Superstar != null;
    }
}
