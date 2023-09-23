namespace RawDeal;
using RawDealView;
using RawDeal.Logic;
using RawDeal.Models;

public class GameInitializationResult
{
    public Player FirstPlayer { get; set; } = null!;
    public Player SecondPlayer { get; set; } = null!;
    public bool IsSuccess { get; set; }
}

public class GameInitializer
{
    private readonly View _view;
    private readonly string _deckFolder;

    public GameInitializer(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
    }

    public GameInitializationResult InitializeGame()
    {
        GameInitializationResult result = new GameInitializationResult();

        DeckLoader.InitializeDeckLoader();

        Deck firstDeck = InitializeFirstDeck();
        if (firstDeck == null)
            return result;

        Deck secondDeck = InitializeSecondDeck();
        if (secondDeck == null)
            return result;

        Player firstPlayer = new Player(firstDeck, _view);
        Player secondPlayer = new Player(secondDeck, _view);

        InitializePlayerHands(firstPlayer, secondPlayer);

        Player startingPlayer = DetermineStartingPlayer(firstPlayer, secondPlayer);
        Player otherPlayer = (startingPlayer == firstPlayer) ? secondPlayer : firstPlayer;

        result.FirstPlayer = startingPlayer;
        result.SecondPlayer = otherPlayer;
        result.IsSuccess = true;

        return result;
    }


    private Deck GetAndValidateDeck()
    {
        Dictionary<string, Superstar> allAvailableSuperstars = SuperstarLoader.LoadSuperstarsIntoDictionary();
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        Deck deck = DeckLoader.LoadDeck(deckPath);
        return DeckValidator.IsValidDeck(deck, allAvailableSuperstars).IsValid ? deck : null;
    }

    private Deck? InitializeFirstDeck()
    {
        Deck? firstDeck = GetAndValidateDeck();
        if (firstDeck == null)
        {
            _view.SayThatDeckIsInvalid();
        }
        return firstDeck;
    }

    private Deck? InitializeSecondDeck()
    {
        Deck? secondDeck = GetAndValidateDeck();
        if (secondDeck == null)
        {
            _view.SayThatDeckIsInvalid();
        }
        return secondDeck;
    }

    private Player DetermineStartingPlayer(Player firstPlayer, Player secondPlayer)
    {
        return firstPlayer.Superstar.SuperstarValue >= secondPlayer.Superstar.SuperstarValue ? firstPlayer : secondPlayer;
    }

    private void InitializePlayerHands(Player firstPlayer, Player secondPlayer)
    {
        for (int i = 0; i < firstPlayer.Superstar.HandSize; i++) firstPlayer.DrawCard();
        
        for (int i = 0; i < secondPlayer.Superstar.HandSize; i++) secondPlayer.DrawCard();
    }
}