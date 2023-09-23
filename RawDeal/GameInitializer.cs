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

        var result = new GameInitializationResult();

        DeckLoader.InitializeDeckLoader();
        
        Deck firstDeck = InitializeDeck("first");
        Deck secondDeck = InitializeDeck("second");
        if (firstDeck == null || secondDeck == null)
        {
            return new GameInitializationResult();
        }


        var (firstPlayer, secondPlayer) = InitializePlayers(firstDeck, secondDeck);
        

        InitializePlayerHands(firstPlayer, secondPlayer);

        var (startingPlayer, otherPlayer) = DeterminePlayerOrder(firstPlayer, secondPlayer);

        return CreateInitializationResult(startingPlayer, otherPlayer);
    }

    private (Player, Player) InitializePlayers(Deck firstDeck, Deck secondDeck)
    {
        Player firstPlayer = new (firstDeck, _view);
        Player secondPlayer = new (secondDeck, _view);
        return (firstPlayer, secondPlayer);
    }

    private GameInitializationResult CreateInitializationResult(Player startingPlayer, Player otherPlayer)
    {
        return new GameInitializationResult
        {
            FirstPlayer = startingPlayer,
            SecondPlayer = otherPlayer,
            IsSuccess = true
        };
    }

    private (Player, Player) DeterminePlayerOrder(Player firstPlayer, Player secondPlayer)
    {
        var startingPlayer = firstPlayer.Superstar.SuperstarValue >= secondPlayer.Superstar.SuperstarValue ? firstPlayer : secondPlayer;
        var otherPlayer = (startingPlayer == firstPlayer) ? secondPlayer : firstPlayer;
        return (startingPlayer, otherPlayer);
    }

    private Deck GetAndValidateDeck()
    {   
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        Deck deck = DeckLoader.LoadDeck(deckPath);
        Dictionary<string, Superstar> allAvailableSuperstars = SuperstarLoader.LoadSuperstarsIntoDictionary();

        var validationResult = DeckValidator.IsValidDeck(deck, allAvailableSuperstars);

        return validationResult.IsValid ? deck : null;
    }

    private Deck? InitializeDeck(string playerOrder)
    {
        var deck = GetAndValidateDeck();
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
