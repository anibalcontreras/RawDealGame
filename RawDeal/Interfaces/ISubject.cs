namespace RawDeal.Interfaces;
using Models;

public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers(string message, Player player);
}
