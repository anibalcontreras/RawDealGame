// using RawDeal.Exceptions;
// using RawDeal.Interfaces;
// using RawDeal.Models;
// using RawDeal.Models.Reversals;
// using RawDealView;
// namespace RawDeal.Controllers;

// public class ReversalController : ISubject
// {
//     private readonly View _view;
//     private readonly ReversalCatalog _reversalCatalog;
//     
//     private readonly List <IObserver> _observers = new List<IObserver>();
//     
//     public ReversalController(View view)
//     {
//         _view = view;
//         _reversalCatalog = new ReversalCatalog(view);
//     }
//     
//     public void RegisterObserver(IObserver observer)
//     {
//         _observers.Add(observer);
//     }
//     public void RemoveObserver(IObserver observer)
//     {
//         _observers.Remove(observer);
//     }
//     public void NotifyObservers(string message, Player player)
//     {
//         Console.WriteLine("Estamos en el notifyObservers");
//         foreach (var observer in _observers)
//         {
//             Console.WriteLine("observer, ", observer);
//             observer.Update(message, player);
//         }
//     }
//
//     public void CanReverseDamage(Player player, Card lastCard)
//     {
//         if (IsPossibleToUseReversal(lastCard))
//         {
//             Reversal reversalCard = _reversalCatalog.GetReversalBy(lastCard.Title);
//             if (reversalCard.CanReverseFromDeck(lastCard, player))
//             {
//                 NotifyObservers("CardReversedByDeck", player);
//                 Console.WriteLine("ACA ESTAMOS");
//                 throw new CardReversedButGameContinuesException();
//             }
//         }
//     }
//
//     private bool IsPossibleToUseReversal(Card reversalCard)
//     {
//         Reversal potentialReversal = _reversalCatalog.GetReversalBy(reversalCard.Title);
//         // No retornar null
//         return potentialReversal != null;
//     }
// }