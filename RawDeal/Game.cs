using RawDealView;
using RawDeal.Models;
using RawDeal.Controllers;
namespace RawDeal;
public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;
    private readonly GameInitializer _gameInitializer;
    private readonly GameplayController _gameplayController;
    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
        _gameInitializer = new GameInitializer(view, deckFolder);
        _gameplayController = new GameplayController(view);
    }
    private GameInitializationResult InitializeGame()
    {
        return _gameInitializer.InitializeGame();
    }
    public void Play()
    {
        GameInitializationResult initResult = InitializeGame();
        if (!initResult.IsSuccess)
            return;

        PlayGame(initResult.FirstPlayer, initResult.SecondPlayer);
    }
    private void PlayGame(Player firstPlayer, Player secondPlayer)
    {
        while (true)
        {
            if (!CheckGameStatus(firstPlayer, secondPlayer))
                break;

            if (!CheckGameStatus(secondPlayer, firstPlayer))
                break;
        }
    }
    private bool CheckGameStatus(Player currentPlayer, Player opponentPlayer)
    {
        return _gameplayController.PlayTurn(currentPlayer, opponentPlayer);
    }
}