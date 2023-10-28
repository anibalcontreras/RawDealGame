using RawDeal.Interfaces;
using RawDealView;
using RawDealView.Options;
using RawDeal.Models;
using RawDeal.Models.Superstars;
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
    public GameplayController(View view)
    {
        _view = view;
        _playerActionsController = new PlayerActionsController(view);
        _superstarAbilityController = new SuperstarAbilityController(view);
        _cardPlayController = new CardPlayController(view);
        _cardPlayController.RegisterObserver(this);
        _cardDisplayController = new CardDisplayController(view);
    }
    public void Update(string message, Player player)
    {
        if (message == "EndGame")
        {
            EndGame(player);
        }
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
                _cardPlayController.HandlePlayCardAction(firstPlayer, secondPlayer);
                if (!GameOn || !_continueTurn) return false;
                break;
            case NextPlay.UseAbility:
                firstPlayer.Superstar.ActivateAbility(firstPlayer, secondPlayer, AbilityActivation.InMenu);
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
        TurnOn = false;
        GameOn = false;
        _view.CongratulateWinner(winningPlayer.Superstar.Name);
    }
}