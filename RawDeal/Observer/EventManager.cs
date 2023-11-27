using RawDeal.Models;

namespace RawDeal.Observer;

public class EventManager
{
    private static EventManager _instance = new EventManager();
    private readonly Dictionary<string, List<IObserver>> _listeners = new Dictionary<string, List<IObserver>>();

    private EventManager() { }
    public static EventManager Instance => _instance;
    public void Subscribe(string eventType, IObserver listener)
    {
        if (!_listeners.ContainsKey(eventType))
        {
            _listeners[eventType] = new List<IObserver>();
        }
        _listeners[eventType].Add(listener);
    }
    
    public void Notify(string eventType, string data, Player player)
    {
        if (_listeners.ContainsKey(eventType))
        {
            foreach (var listener in _listeners[eventType])
            {
                listener.Update(data, player);
            }
        }
    }
}