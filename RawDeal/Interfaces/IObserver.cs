namespace RawDeal.Interfaces;
using Models;
public interface IObserver
{
    void Update(String message, Player player);
}