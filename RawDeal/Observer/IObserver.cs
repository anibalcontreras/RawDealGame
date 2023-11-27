namespace RawDeal.Observer;
using Models;
public interface IObserver
{
    void Update(string message, Player player);
}