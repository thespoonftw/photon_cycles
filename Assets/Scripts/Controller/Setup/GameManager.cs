using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

public class GameManager
{
    private const int practiceSceneIndex = 1;

    private readonly Updater updater;
    private readonly Resourcer resourcer;
    private readonly NetworkManager network;

    private List<Player> players = new();
    private int levelSelectIndex;
    private IInputController leadInput;

    public GameManager(Resourcer resourcer, Updater updater, NetworkManager network)
    {
        this.resourcer = resourcer;
        this.updater = updater;
        this.network = network;
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
            LoadLobby();
        }
        else if (selection == MainMenuSelection.Join)
        {
            var joinMenu = new JoinMenuController(resourcer, leadInput, network, updater);
        }
    }

    private void LoadLobby()
    {
        updater.LoadScene(practiceSceneIndex, StartLobby);
    }

    private void StartLobby()
    {
        var spawnLocater = updater.FindSpawnLocater();
        var bikeManager = new BikeManager(spawnLocater, resourcer, updater);
        new LobbyManager(bikeManager, FinishLobby, updater, players, resourcer, leadInput);
    }

    private void FinishLobby(List<Player> players)
    {
        this.players = players;
        updater.UnloadScene(practiceSceneIndex, StartLevelSelect);        
    }

    private void StartLevelSelect()
    {
        var levelSelectManager = new LevelSelectController(updater, leadInput, resourcer, FinishLevelSelect);
    }

    private void FinishLevelSelect(int levelSelectIndex)
    {
        this.levelSelectIndex = levelSelectIndex;
        updater.LoadScene(levelSelectIndex, StartLiveGame);
    }

    private void StartLiveGame()
    {
        var spawnLocater = updater.FindSpawnLocater();
        var bikeManager = new BikeManager(spawnLocater, resourcer, updater);
        var liveGameManager = new LiveGameManager(bikeManager, players, FinishLiveGame, updater);
    }

    private void FinishLiveGame()
    {
        updater.UnloadScene(levelSelectIndex, LoadLobby);
    }

    private void LoadScene(int sceneIndex, Action postLoadAction)
    {
        var m = network.SceneManager.LoadScene("this", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        network.SceneManager.
    }
}
