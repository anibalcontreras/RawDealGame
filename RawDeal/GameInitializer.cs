namespace RawDeal;
using RawDealView;
using RawDeal.Logic;
using RawDeal.Models;
using RawDealView;

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

        DeckLoader.InitializeDeckLoader(_view);

        Deck? firstDeck = InitializeDeck(_view);
        if (firstDeck == null)
            return result;

        Deck? secondDeck = InitializeDeck(_view);
        if (secondDeck == null)
            return result;

        SetPlayers(firstDeck, secondDeck, out Player startingPlayer, out Player otherPlayer);

        result.FirstPlayer = startingPlayer;
        result.SecondPlayer = otherPlayer;
        result.IsSuccess = true;

        return result;
    }

    private void SetPlayers(Deck firstDeck, Deck secondDeck, out Player startingPlayer, out Player otherPlayer)
    {
        Player firstPlayer = new Player(firstDeck, _view);
        Player secondPlayer = new Player(secondDeck, _view);

        InitializePlayerHands(firstPlayer, secondPlayer);

        startingPlayer = DetermineStartingPlayer(firstPlayer, secondPlayer);
        otherPlayer = (startingPlayer == firstPlayer) ? secondPlayer : firstPlayer;
    }


    private Deck GetAndValidateDeck(View view)
    {
        Dictionary<string, Superstar> allAvailableSuperstars = SuperstarLoader.LoadSuperstarsIntoDictionary(view);
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        Deck deck = DeckLoader.LoadDeck(deckPath);
        return DeckValidator.IsValidDeck(deck, allAvailableSuperstars).IsValid ? deck : null;
    }

    private Deck? InitializeDeck(View view)
    {
        Deck? deck = GetAndValidateDeck(view);
        if (deck == null)
        {
            _view.SayThatDeckIsInvalid();
        }
        return deck;
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