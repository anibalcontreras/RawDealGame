using RawDeal.Controllers;
using RawDeal.Exceptions;
using RawDeal.Loaders;
using RawDeal.Models;
using RawDeal.Models.Superstars;
using RawDealView;
namespace RawDeal;
public class GameInitializer
{
    private readonly View _view;
    private readonly string _deckFolder;
    private readonly PlayerActionsController _playerActionsController;
    public GameInitializer(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
        _playerActionsController = new PlayerActionsController(view);
    }
    public GameInitializationResult InitializeGame()
    {
        GameInitializationResult result = new GameInitializationResult();

        DeckLoader.InitializeDeckLoader(_view);

        try
        {
            Deck firstDeck = InitializeDeck(_view);
            Deck secondDeck = InitializeDeck(_view);
            
            (Player StartingPlayer, Player OtherPlayer) players = SetPlayers((firstDeck, secondDeck));
            result.FirstPlayer = players.StartingPlayer;
            result.SecondPlayer = players.OtherPlayer;
            result.IsSuccess = true;
        }
        catch (InvalidDeckException)
        {
            _view.SayThatDeckIsInvalid();
        }

        return result;
    }
    private (Player StartingPlayer, Player OtherPlayer) SetPlayers((Deck FirstDeck, Deck SecondDeck) decks)
    {
        Player firstPlayer = new Player(decks.FirstDeck, _view);
        Player secondPlayer = new Player(decks.SecondDeck, _view);

        InitializePlayerHands(firstPlayer, secondPlayer);

        Player startingPlayer = DetermineStartingPlayer(firstPlayer, secondPlayer);
        Player otherPlayer = (startingPlayer == firstPlayer) ? secondPlayer : firstPlayer;

        return (startingPlayer, otherPlayer);
    }
    private Deck GetAndValidateDeck(View view)
    {
        Dictionary<string, Superstar> allAvailableSuperstars = SuperstarLoader.LoadSuperstarsIntoDictionary(view);
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        Deck deck = DeckLoader.LoadDeck(deckPath);
    
        if (!DeckValidator.IsValidDeck(deck, allAvailableSuperstars).IsValid)
        {
            throw new InvalidDeckException();
        }
        return deck;
    }
    private Deck InitializeDeck(View view)
    {
        return GetAndValidateDeck(view);
    }
    private Player DetermineStartingPlayer(Player firstPlayer, Player secondPlayer)
    {
        Superstar firstPlayerSuperstar = firstPlayer.Superstar;
        Superstar secondPlayerSuperstar = secondPlayer.Superstar;

        int firstPlayerSuperstarValue = firstPlayerSuperstar.SuperstarValue;
        int secondPlayerSuperstarValue = secondPlayerSuperstar.SuperstarValue;

        return firstPlayerSuperstarValue >= secondPlayerSuperstarValue ? firstPlayer : secondPlayer;
    }
    private void InitializePlayerHands(Player firstPlayer, Player secondPlayer)
    {
        Superstar firstPlayerSuperstar = firstPlayer.Superstar;
        Superstar secondPlayerSuperstar = secondPlayer.Superstar;
        
        int firstPlayerHandSize = firstPlayerSuperstar.HandSize;
        int secondPlayerHandSize = secondPlayerSuperstar.HandSize;

        for (int i = 0; i < firstPlayerHandSize; i++) 
        {
            _playerActionsController.DrawCard(firstPlayer);
        }
        
        for (int i = 0; i < secondPlayerHandSize; i++) 
        {
            _playerActionsController.DrawCard(secondPlayer);
        }
    }
}