
public class GameController
{
    private readonly Game game;

    public GameController(Game game)
    {
        this.game = game;
        StartTitleScreen();
    }

    private void StartTitleScreen()
    {
        new TitleScreenController(game, StartMenuScreen);
    }

    private void StartMenuScreen()
    {
        new MainMenuController(game, FinishMenuScreen);
    }

    private void FinishMenuScreen(MainMenuSelection selection)
    {
        if (selection == MainMenuSelection.Play)
        {
            new ServerController(game);
            new LobbyController(game, new(game.Network));
        }
        else if (selection == MainMenuSelection.Join)
        {
            new JoinMenuController(game, JoinAsClient);
        }
    }

    private void JoinAsClient()
    {
        new LobbyController(game, new(game.Network));
    }
}
