using RawDealView;
using RawDeal.Logic;
using RawDeal.Models;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;

    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
    }

    public void Play()
    {
        InitializeGame();

        var firstDeck = GetAndValidateDeck();
        if (firstDeck == null)
        {
        _view.SayThatDeckIsInvalid();
        return;
        }
        var secondDeck = GetAndValidateDeck();
        if (secondDeck == null)
        {
        _view.SayThatDeckIsInvalid();
        return;
        }

        var firstPlayer = new Player(firstDeck);
        var secondPlayer = new Player(secondDeck);

         // Inicializa las manos de los jugadores
        for (int i = 0; i < firstPlayer.Superstar.HandSize; i++) firstPlayer.DrawCard();
        for (int i = 0; i < secondPlayer.Superstar.HandSize; i++) secondPlayer.DrawCard();

        var startingPlayer = DetermineStartingPlayer(firstPlayer, secondPlayer);

        InitializeGameWithStartingPlayer(startingPlayer, startingPlayer == firstPlayer ? secondPlayer : firstPlayer);
        // StartGame(firstDeck, secondDeck);
    }

    private void InitializeGame()
    {
        DeckLoader.InitializeDeckLoader();
    }

    private Deck GetAndValidateDeck()
    {
        var allAvailableSuperstars = SuperstarLoader.LoadSuperstarsIntoDictionary();
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        Deck deck = DeckLoader.LoadDeck(deckPath);
        var validationResult = DeckValidator.IsValidDeck(deck, allAvailableSuperstars);

        return validationResult.IsValid ? deck : null;
    }

    // private void StartGame(Deck firstDeck, Deck secondDeck)
    // {
    //     var startingPlayer = DetermineStartingPlayer(firstDeck, secondDeck);

    //     _view.AskUserWhatToDoWhenHeCannotUseHisAbility();

    //     // DetermineAndAnnounceWinner(firstDeck, secondDeck);
    // }

    private Player DetermineStartingPlayer(Player firstPlayer, Player secondPlayer)
    {
        return firstPlayer.Superstar.SuperstarValue >= secondPlayer.Superstar.SuperstarValue ? firstPlayer : secondPlayer;
    }

    private void InitializeGameWithStartingPlayer(Player startingPlayer, Player otherPlayer)
    {
        startingPlayer.DrawCard();

        var startingPlayerInfo = startingPlayer.ToPlayerInfo();
        var otherPlayerInfo = otherPlayer.ToPlayerInfo();

        _view.SayThatATurnBegins(startingPlayer.Superstar.Name);
        _view.ShowGameInfo(startingPlayerInfo, otherPlayerInfo);
    }

    // private PlayerInfo DetermineStartingPlayer(Deck firstDeck, Deck secondDeck)
    // {
    //     return firstDeck.Superstar.SuperstarValue >= secondDeck.Superstar.SuperstarValue
    //         ? InitializePlayerInfo(firstDeck, secondDeck)
    //         : InitializePlayerInfo(secondDeck, firstDeck);
    // }

    // private PlayerInfo InitializePlayerInfo(Deck winnerDeck, Deck loserDeck)
    // {
    //     var startingPlayer = CreatePlayerInfoFromDeck(winnerDeck, extraHandSize: 1);
    //     var otherPlayer = CreatePlayerInfoFromDeck(loserDeck);

    //     _view.SayThatATurnBegins(startingPlayer.Name);
    //     _view.ShowGameInfo(startingPlayer, otherPlayer);

    //     return startingPlayer;
    // }

    // private PlayerInfo CreatePlayerInfoFromDeck(Deck deck, int extraHandSize = 0)
    // {
    //     return new PlayerInfo(
    //         deck.Superstar.Name,
    //         0,
    //         deck.Superstar.HandSize + extraHandSize,
    //         60 - deck.Superstar.HandSize - extraHandSize);
    // }

    // private void DetermineAndAnnounceWinner(Deck firstDeck, Deck secondDeck)
    // {
    //     string winner = DetermineWinner(firstDeck, secondDeck);
    //     _view.CongratulateWinner(winner);
    // }

    // private string DetermineWinner(Deck firstDeck, Deck secondDeck)
    // {
    //     return firstDeck.Superstar.SuperstarValue >= secondDeck.Superstar.SuperstarValue
    //         ? secondDeck.Superstar.Name
    //         : firstDeck.Superstar.Name;
    // }
}