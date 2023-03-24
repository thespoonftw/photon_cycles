using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager
{
    private const int practiceSceneIndex = 1;

    private readonly Updater updater;
    private readonly Resourcer resourcer;

    private List<Player> players = new();
    private int levelSelectIndex;
    private IInputController leadInput;

    public GameManager(Resourcer resourcer, Updater updater)
    {
        this.resourcer = resourcer;
        this.updater = updater;
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
        if (selection == MainMenuSelection.Local)
        {
            LoadPlayerSetup();
        }
    }

    private void LoadPlayerSetup()
    {
        updater.LoadScene(practiceSceneIndex, StartPlayerSetup);
    }

    private void StartPlayerSetup()
    {
        var spawnLocater = updater.FindSpawnLocater();
        var bikeManager = new BikeManager(spawnLocater, resourcer, updater);
        new PlayerSetupManager(bikeManager, FinishPlayerSetup, updater, players, resourcer, leadInput);
    }

    private void FinishPlayerSetup(List<Player> players)
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
        updater.UnloadScene(levelSelectIndex, LoadPlayerSetup);
    }
}
