namespace RawDeal.Interfaces;
using Models;
public interface IObserver
{
    void Update(string message, Player player);
}