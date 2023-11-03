using RawDealView;
using RawDeal.Models;
using RawDeal.Exceptions;
using RawDeal.Models.Effects;
using RawDeal.Interfaces;
using RawDeal.Utilities;
namespace RawDeal.Controllers;
public class CardPlayController : ISubject
{
    private readonly View _view;
    private readonly EffectCatalog _effectCatalog;
    private readonly List <IObserver> _observers = new List<IObserver>();
    private Player CurrentPlayer { get; set; }
    private Player Opponent { get; set; }
    private List<Card> PlayableCards { get; set; }
    private int SelectedPlayIndex { get; set; }
    public CardPlayController(View view)
    {
        _view = view;
        _effectCatalog = new EffectCatalog(view);
    }
    public void RegisterObserver(IObserver observer)
    {
        _observers.Add(observer);
    }
    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }
    public void NotifyObservers(string message, Player player)
    {
        foreach (var observer in _observers)
        {
            observer.Update(message, player);
        }
    }
    public void HandlePlayCardAction(Player firstPlayer, Player secondPlayer)
    {
        InitializePlayers(firstPlayer, secondPlayer);
        if (AttemptToPlayCard())
        {
            ExecuteCardPlay();
        }
    }
    private void InitializePlayers(Player firstPlayer, Player secondPlayer)
    {
        CurrentPlayer = firstPlayer;
        Opponent = secondPlayer;
        PlayableCards = PlayUtility.GetPlayableCards(CurrentPlayer.GetHand(), CurrentPlayer.Fortitude);
    }
    private bool AttemptToPlayCard()
    {
        List<string> formatPlayableCards=
            PlayUtility.GetFormattedPlayableCards(PlayableCards, CurrentPlayer.Fortitude);
        SelectedPlayIndex = _view.AskUserToSelectAPlay(formatPlayableCards);
        return SelectedPlayIndex != -1;
    }
    private Card GetPlayedCard()
    {
        try
        {
            Card card = (Card)GetSelectedPlay().CardInfo;
            return card;
        }
        catch (InvalidCastException)
        {
            throw new InvalidCardConversionException();
        }
    }
    private string GetPlayedAs()
    {
        return GetSelectedPlay().PlayedAs;
    }
    private Play GetSelectedPlay()
    {
        List<Play> playablePlays = PlayUtility.GetPlayablePlays(CurrentPlayer.GetHand(), CurrentPlayer.Fortitude);
        return playablePlays[SelectedPlayIndex];
    }
    private void ApplyCardEffect()
    {
        Card playedCard = GetPlayedCard();
        string playedAs = GetPlayedAs();
        Effect cardEffect = _effectCatalog.GetEffectBy(playedCard.Title, playedAs);
        bool hasLost = cardEffect.Apply(CurrentPlayer, Opponent, GetPlayedCard());
        if (hasLost)
            EndGame(CurrentPlayer);
    }
    private void ExecuteCardPlay()
    {
        AnnounceAttemptToPlayCard();
        AnnounceSuccessfulCardPlay();
        ApplyCardEffect();
    }
    private void AnnounceAttemptToPlayCard()
    {
        string selectedPlay =
            PlayUtility.GetFormattedPlayableCards(PlayableCards, CurrentPlayer.Fortitude)[SelectedPlayIndex];
        _view.SayThatPlayerIsTryingToPlayThisCard(CurrentPlayer.Superstar.Name, selectedPlay);
    }
    private void AnnounceSuccessfulCardPlay()
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }
    private void EndGame(Player winningPlayer)
    {
        NotifyObservers("EndGame", winningPlayer);
    }
}
