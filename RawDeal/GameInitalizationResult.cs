using RawDeal.Models;
namespace RawDeal;
public class GameInitializationResult
{
    public Player FirstPlayer { get; set; }
    public Player SecondPlayer { get; set; }
    public bool IsSuccess { get; set; }
}