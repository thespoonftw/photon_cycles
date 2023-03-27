using System.Collections.Generic;
using Unity.Netcode;

public class GameManager
{
    private readonly Updater updater;
    private readonly Resourcer resourcer;
    private readonly NetworkManager network;

    private List<Player> players = new();
    private IInputController leadInput;
    private LevelManager levelManager;

    public GameManager(Resourcer resourcer, Updater updater, NetworkManager network)
    {
        this.resourcer = resourcer;
        this.updater = updater;
        this.network = network;
        levelManager = new LevelManager(network, resourcer);
        StartTitleScreen();
    }

    private void StartTitleScreen()
    {
        var titleScreen = new TitleScreenController(resourcer, updater, StartMenuScreen);
    }

    private void StartMenuScreen(IInputController leadInput)
    {
        this.leadInput = leadInput;
        var menuScreen = new MainMenuController(resourcer, updater, leadInput, FinishMenuScreen);
    }

    private void FinishMenuScreen(MainMenuSelection selection)
    {
        if (selection == MainMenuSelection.Play)
        {
            HostLobby();
        }
        else if (selection == MainMenuSelection.Join)
        {
            var joinMenu = new JoinMenuController(resourcer, leadInput, network, updater, JoinLobby);
        }
    }

    private void HostLobby()
    {
        network.StartHost();
        LoadLobby();
    }

    private void LoadLobby()
    {
        levelManager.LoadLevel(resourcer.practiceLevel, StartLobby);
    }

    private void JoinLobby()
    {
        network.StartClient();
    }

    private void StartLobby()
    {
        new LobbyManager(FinishLobby, updater, players, resourcer, leadInput, network);
    }

    private void FinishLobby(List<Player> players)
    {
        this.players = players;
        StartLevelSelect();      
    }

    private void StartLevelSelect()
    {
        var levelSelectManager = new LevelSelectController(updater, leadInput, resourcer, StartLiveGame, levelManager);
    }

    private void StartLiveGame()
    {
        var liveGameManager = new LiveGameManager(players, LoadLobby, updater, resourcer);
    }
}
