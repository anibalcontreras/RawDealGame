using RawDealView;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;

    private readonly GameInitializer _gameInitializer;

    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
        _gameInitializer = new GameInitializer(view, deckFolder);
    }

    public void Play()
    {
        _gameInitializer.InitializeGame();
    }
}