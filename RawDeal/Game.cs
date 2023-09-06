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
        var initResult = _gameInitializer.InitializeGame();
        if (!initResult.IsSuccess)
            return;
        
        Player firstPlayer = initResult.FirstPlayer;
        Player secondPlayer = initResult.SecondPlayer;

        while (true)
        {
            _playerTurn.PlayTurn(firstPlayer, secondPlayer);
            _playerTurn.PlayTurn(secondPlayer, firstPlayer);
        }

    }
}