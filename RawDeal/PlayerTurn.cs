namespace RawDeal;

using RawDealView;

public class PlayerTurn
{
    private readonly View _view;

    public PlayerTurn(View view)
    {
        _view = view;
    }

    public void PlayTurn(Player firstPlayer, Player secondPlayer)
    {

        firstPlayer.DrawCard();

        var firstPlayerInfo = firstPlayer.ToPlayerInfo();
        var secondPlayerInfo = secondPlayer.ToPlayerInfo();

        _view.SayThatATurnBegins(firstPlayer.Superstar.Name);
        _view.ShowGameInfo(firstPlayerInfo, secondPlayerInfo);

        _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }
}