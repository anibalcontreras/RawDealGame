using RawDeal.Observer;
using RawDealView;
using RawDealView.Options;
using RawDeal.Models;
namespace RawDeal.Controllers;
public class GameplayController : IObserver
{
    private bool _continueTurn = true;
    private readonly View _view;
    private Player CurrentPlayer { get; set; }
    private Player Opponent { get; set; }
    private bool GameOn { get; set; } = true;
    private bool TurnOn { get; set; } = true;
    private readonly PlayerActionsController _playerActionsController;
    private readonly SuperstarAbilityController _superstarAbilityController;
    private readonly CardPlayController _cardPlayController;
    private readonly CardDisplayController _cardDisplayController;
    private readonly EventManager _eventManager;
    
    public GameplayController(View view)
    {
        _view = view;
        EventManager eventManager = EventManager.Instance;
        _playerActionsController = new PlayerActionsController(view);
        _cardPlayController = new CardPlayController(view);
        eventManager.Subscribe("CardReversedByDeck", this);
        eventManager.Subscribe("EndGame", this);
        eventManager.Subscribe("CardReversedByHand", this);
        _superstarAbilityController = new SuperstarAbilityController(view);
        _cardDisplayController = new CardDisplayController(view);
    }
    public void Update(string message, Player player)
    {
        switch (message)
        {
            case "CardReversedByDeck":
            case "CardReversedByHand":
                HandleCardReversal();
                break;
            case "EndGame":
                EndGame(player);
                break;
        }
    }
    
    private void HandleCardReversal()
    {
        _superstarAbilityController.ResetAbilityUsage(Opponent);
        _superstarAbilityController.ResetAbilityUsage(CurrentPlayer);
        _continueTurn = false;
    }

    public bool PlayTurn(Player firstPlayer, Player secondPlayer)
    {
        StartPlayerTurn(firstPlayer);
        _superstarAbilityController.ActivateSuperstarsAbility(firstPlayer, secondPlayer);
        ExecutePlayerActions(firstPlayer, secondPlayer);
        return GameOn;
    }
    
    private void StartPlayerTurn(Player player)
    {
        _playerActionsController.DrawCard(player);
        _view.SayThatATurnBegins(player.Superstar.Name);
    }
    
    private void ExecutePlayerActions(Player firstPlayer, Player secondPlayer)
    {
        while (_continueTurn)
        {
            _continueTurn = HandleTurnActions(firstPlayer, secondPlayer);
            CurrentPlayer = firstPlayer;
            Opponent = secondPlayer;
        }
        _continueTurn = true;
    }
    
    private bool HandleTurnActions(Player firstPlayer, Player secondPlayer)
    {
        _view.ShowGameInfo(firstPlayer.PlayerInfo(), secondPlayer.PlayerInfo());
        
        NextPlay turnActionsSelections = 
            _superstarAbilityController.DetermineIfSuperstarCanActivateHisAbility(firstPlayer);
        
        switch(turnActionsSelections)
        {
            case NextPlay.ShowCards:
                _cardDisplayController.HandleShowCardsActions(firstPlayer, secondPlayer);
                break;
            case NextPlay.PlayCard:
                _playerActionsController.InitializePlayers(firstPlayer, secondPlayer);
                _cardPlayController.HandlePlayCardAction(firstPlayer, secondPlayer);
                if (!GameOn) return false;
                break;
            case NextPlay.UseAbility:
                _superstarAbilityController.ActivateAbilityInMenu(firstPlayer, secondPlayer);
                break;
            case NextPlay.EndTurn:
                HandleEndTurnAction(firstPlayer, secondPlayer);
                return false;
            case NextPlay.GiveUp:
                HandleGiveUpAction(secondPlayer);
                return false;
        }
        return _continueTurn;
    }
    private void HandleEndTurnAction(Player firstPlayer, Player secondPlayer)
    {
        CurrentPlayer = firstPlayer;
        Opponent = secondPlayer;

        if (Opponent.HasEmptyArsenal())
            EndGame(CurrentPlayer);
        if (CurrentPlayer.HasEmptyArsenal())
            EndGame(Opponent);
        TurnOn = false;
        _superstarAbilityController.ResetAbilityUsage(CurrentPlayer);
    }
    private void HandleGiveUpAction(Player opponentPlayer)
    {
        EndGame(opponentPlayer);
    }
    private void EndGame(Player winningPlayer)
    {
        _view.CongratulateWinner(winningPlayer.Superstar.Name);
        TurnOn = false;
        GameOn = false;
    }
}