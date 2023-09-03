using RawDealView;
using RawDeal.Logic;
using RawDeal.Models;

namespace RawDeal;

public class Game
{
    private View _view;
    private string _deckFolder;

    public List<Card> AvailableCards { get; private set; } = new List<Card>();
    public List<Superstar> AvailableSuperstars { get; private set; } = new List<Superstar>();
    
    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
    }

    public void Play()
    {
        InitializeGame();
        // var allAvailableSuperstars = SuperstarLoader.LoadSuperstarsIntoDictionary();

        Deck firstDeck = GetAndValidateDeck();
        if (firstDeck == null)
        {
            return;
        }

        Deck secondDeck = GetAndValidateDeck();
        if (secondDeck == null)
        {
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

    // private Deck GetAndValidateDeck()
    // {
    //     var allAvailableSuperstars = SuperstarLoader.LoadSuperstarsIntoDictionary();
    //     string deckPath = _view.AskUserToSelectDeck(_deckFolder);
    //     Deck deck = DeckLoader.LoadDeck(deckPath);
    //     var validationResult = DeckValidator.IsValidDeck(deck, allAvailableSuperstars);

    //     return validationResult.IsValid ? deck : null;
    // }

    private void StartGame(Deck firstDeck, Deck secondDeck)
    {
        WhoStarts(firstDeck, secondDeck);

        _view.AskUserWhatToDoWhenItIsNotPossibleToUseItsAbility();

        WhoWins(firstDeck, secondDeck);
    }

    private PlayerInfo WhoStarts(Deck firstDeck, Deck secondDeck)
    {
        PlayerInfo startingPlayer;
        PlayerInfo otherPlayer;

        

        if (firstDeck.Superstar.SuperstarValue >= secondDeck.Superstar.SuperstarValue)
        {
            startingPlayer = new PlayerInfo(firstDeck.Superstar.Name, 0, firstDeck.Superstar.HandSize + 1, 60 - firstDeck.Superstar.HandSize - 1);
            otherPlayer = new PlayerInfo(secondDeck.Superstar.Name, 0, secondDeck.Superstar.HandSize, 60 - secondDeck.Superstar.HandSize);
        }
        else
        {
            startingPlayer = new PlayerInfo(secondDeck.Superstar.Name, 0, secondDeck.Superstar.HandSize + 1, 60 - secondDeck.Superstar.HandSize - 1);
            otherPlayer = new PlayerInfo(firstDeck.Superstar.Name, 0, firstDeck.Superstar.HandSize, 60 - firstDeck.Superstar.HandSize);
        }

        _view.SayThatATurnBegins(startingPlayer.Name);
        _view.ShowGameInfo(startingPlayer, otherPlayer);
        return startingPlayer;
    }

    private string DetermineWinner(Deck firstDeck, Deck secondDeck)
    {
        if (firstDeck.Superstar.SuperstarValue >= secondDeck.Superstar.SuperstarValue)
        {
            return secondDeck.Superstar.Name;
        }
        else
        {
            return firstDeck.Superstar.Name;
        }
    }


    private void WhoWins(Deck firstDeck, Deck secondDeck)
    {
        string winner = DetermineWinner(firstDeck, secondDeck);
        _view.CongratulateWinner(winner);
    }
}