namespace RawDeal;
using RawDealView;
using RawDeal.Logic;
using RawDeal.Models;
public class GameInitializer
{
    private readonly View _view;
    private readonly string _deckFolder;

    public GameInitializer(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
    }

    public void InitializeGame()
    {
        DeckLoader.InitializeDeckLoader();
        
        var firstDeck = InitializeFirstDeck();
        if (firstDeck == null)
            return;
        var secondDeck = InitializeSecondDeck();
        if (secondDeck == null)
            return;
        var firstPlayer = new Player(firstDeck);
        var secondPlayer = new Player(secondDeck);

        InitializePlayerHands(firstPlayer, secondPlayer);

        var startingPlayer = DetermineStartingPlayer(firstPlayer, secondPlayer);

        InitializeGameWithStartingPlayer(startingPlayer, startingPlayer == firstPlayer ? secondPlayer : firstPlayer);
    }

    private Deck GetAndValidateDeck()
    {
        var allAvailableSuperstars = SuperstarLoader.LoadSuperstarsIntoDictionary();
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        Deck deck = DeckLoader.LoadDeck(deckPath);
        var validationResult = DeckValidator.IsValidDeck(deck, allAvailableSuperstars);

        return validationResult.IsValid ? deck : null;
    }

    private Deck? InitializeFirstDeck()
    {
        var firstDeck = GetAndValidateDeck();
        if (firstDeck == null)
        {
            _view.SayThatDeckIsInvalid();
        }
        return firstDeck;
    }

    private Deck? InitializeSecondDeck()
    {
        var secondDeck = GetAndValidateDeck();
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


    private void InitializeGameWithStartingPlayer(Player startingPlayer, Player otherPlayer)
    {
        startingPlayer.DrawCard();

        var startingPlayerInfo = startingPlayer.ToPlayerInfo();
        var otherPlayerInfo = otherPlayer.ToPlayerInfo();

        _view.SayThatATurnBegins(startingPlayer.Superstar.Name);
        _view.ShowGameInfo(startingPlayerInfo, otherPlayerInfo);
    }
}