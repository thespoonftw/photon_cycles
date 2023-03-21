using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int practiceSceneIndex = 1;

    [SerializeField] BikeManager bikeManager;
    [SerializeField] GameObject playerSetupCanvas;
    [SerializeField] LiveGameManager liveGameManager;
    [SerializeField] Updater updater;
    [SerializeField] LevelSelectCanvas levelSelectCanvas;

    private PlayerSetupManager playerSetupManager;
    private LevelSelectManager levelSelectManager;
    private List<Player> players;

    IEnumerator Start()
    {
        yield return SceneManager.LoadSceneAsync(practiceSceneIndex, LoadSceneMode.Additive);
        var practiceScene = SceneManager.GetSceneByBuildIndex(practiceSceneIndex);
        SceneManager.SetActiveScene(practiceScene);

        var spawnLocater = FindObjectOfType<SpawnLocater>();
        bikeManager.Init(spawnLocater);

        playerSetupCanvas.SetActive(true);
        playerSetupManager = new PlayerSetupManager(bikeManager, FinishPlayerSetup, updater, new());
    }

    private void FinishPlayerSetup(List<Player> players) => StartCoroutine(FinishPlayerSetupAsync(players));

    private IEnumerator FinishPlayerSetupAsync(List<Player> players)
    {
        this.players = players;
        playerSetupCanvas.SetActive(false);
        yield return SceneManager.UnloadSceneAsync(practiceSceneIndex);

        levelSelectManager = new LevelSelectManager(updater, players.First(), levelSelectCanvas, FinishLevelSelect);
    }

    private void FinishLevelSelect(int levelSelectIndex) => StartCoroutine(FinishLevelSelectAsync(levelSelectIndex));

    private IEnumerator FinishLevelSelectAsync(int levelSelectIndex)
    {
        yield return SceneManager.LoadSceneAsync(levelSelectIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelSelectIndex));

        var spawnLocater = FindObjectOfType<SpawnLocater>();
        bikeManager.Init(spawnLocater);

        liveGameManager.Init(bikeManager, players, EndGame);
    }

    private void EndGame()
    {
        StartCoroutine(EndGameAsync());
    }

    private IEnumerator EndGameAsync()
    {
        bikeManager.RemoveAllPlayers();

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        yield return SceneManager.LoadSceneAsync(practiceSceneIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(practiceSceneIndex));

        var spawnLocater = FindObjectOfType<SpawnLocater>();
        bikeManager.Init(spawnLocater);

        playerSetupCanvas.SetActive(true);
        playerSetupManager = new PlayerSetupManager(bikeManager, FinishPlayerSetup, updater, players);
    }
}
