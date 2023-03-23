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

    public GameManager(Resourcer resourcer, Updater updater)
    {
        this.resourcer = resourcer;
        this.updater = updater;
        LoadPlayerSetup();
    }

    private void LoadPlayerSetup()
    {
        updater.LoadScene(practiceSceneIndex, StartPlayerSetup);
    }

    private void StartPlayerSetup()
    {
        var spawnLocater = updater.FindSpawnLocater();
        var bikeManager = new BikeManager(spawnLocater, resourcer, updater);
        new PlayerSetupManager(bikeManager, FinishPlayerSetup, updater, players, resourcer);
    }

    private void FinishPlayerSetup(List<Player> players)
    {
        this.players = players;
        updater.UnloadScene(practiceSceneIndex, StartLevelSelect);        
    }

    private void StartLevelSelect()
    {
        var levelSelectManager = new LevelSelectController(updater, players.First(), resourcer, FinishLevelSelect);
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
    {;
        updater.UnloadScene(levelSelectIndex, LoadPlayerSetup);
    }
}
