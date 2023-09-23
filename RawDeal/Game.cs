using RawDealView;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;

    private readonly GameInitializer _gameInitializer;
    private readonly PlayerTurn _playerTurn;

    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
        _gameInitializer = new GameInitializer(view, deckFolder);
        _playerTurn = new PlayerTurn(view);
    }

    public void Play()
    {
        var initResult = InitializeGame();
        if (!initResult.IsSuccess)
            return;

        PlayGame(initResult.FirstPlayer, initResult.SecondPlayer);
    }

    private GameInitializationResult InitializeGame()
    {
        return _gameInitializer.InitializeGame();
    }

    private void PlayGame(Player firstPlayer, Player secondPlayer)
    {
        while (_playerTurn.GameOn)
        {
            PlayTurnAndCheckGameStatus(firstPlayer, secondPlayer);
            if (!_playerTurn.GameOn)
                break;

            PlayTurnAndCheckGameStatus(secondPlayer, firstPlayer);
        }
    }

    private void PlayTurnAndCheckGameStatus(Player currentPlayer, Player opponentPlayer)
    {
        _playerTurn.PlayTurn(currentPlayer, opponentPlayer);
    }
}