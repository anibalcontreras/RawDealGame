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
        StartGame(firstDeck, secondDeck);
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

    private void StartGame(Deck firstDeck, Deck secondDeck)
    {
        var startingPlayer = DetermineStartingPlayer(firstDeck, secondDeck);

        _view.AskUserWhatToDoWhenItIsNotPossibleToUseItsAbility();

        DetermineAndAnnounceWinner(firstDeck, secondDeck);
    }

    private PlayerInfo DetermineStartingPlayer(Deck firstDeck, Deck secondDeck)
    {
        return firstDeck.Superstar.SuperstarValue >= secondDeck.Superstar.SuperstarValue
            ? InitializePlayerInfo(firstDeck, secondDeck)
            : InitializePlayerInfo(secondDeck, firstDeck);
    }

    private PlayerInfo InitializePlayerInfo(Deck winnerDeck, Deck loserDeck)
    {
        var startingPlayer = CreatePlayerInfoFromDeck(winnerDeck, extraHandSize: 1);
        var otherPlayer = CreatePlayerInfoFromDeck(loserDeck);

        _view.SayThatATurnBegins(startingPlayer.Name);
        _view.ShowGameInfo(startingPlayer, otherPlayer);

        return startingPlayer;
    }

    private PlayerInfo CreatePlayerInfoFromDeck(Deck deck, int extraHandSize = 0)
    {
        return new PlayerInfo(
            deck.Superstar.Name,
            0,
            deck.Superstar.HandSize + extraHandSize,
            60 - deck.Superstar.HandSize - extraHandSize);
    }

    private void DetermineAndAnnounceWinner(Deck firstDeck, Deck secondDeck)
    {
        string winner = DetermineWinner(firstDeck, secondDeck);
        _view.CongratulateWinner(winner);
    }

    private string DetermineWinner(Deck firstDeck, Deck secondDeck)
    {
        return firstDeck.Superstar.SuperstarValue >= secondDeck.Superstar.SuperstarValue
            ? secondDeck.Superstar.Name
            : firstDeck.Superstar.Name;
    }
}