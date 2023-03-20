using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int practiceSceneIndex = 1;

    [SerializeField] BikeManager bikeManager;
    [SerializeField] PlayerSetupManager playerSetupManager;
    [SerializeField] GameObject playerSetupCanvas;
    [SerializeField] LiveGameManager liveGameManager;

    IEnumerator Start()
    {
        yield return SceneManager.LoadSceneAsync(practiceSceneIndex, LoadSceneMode.Additive);
        var practiceScene = SceneManager.GetSceneByBuildIndex(practiceSceneIndex);
        SceneManager.SetActiveScene(practiceScene);

        var spawnLocater = FindObjectOfType<SpawnLocater>();
        bikeManager.Init(spawnLocater);

        playerSetupManager.enabled = true;
        playerSetupCanvas.SetActive(true);
        playerSetupManager.Init(bikeManager, StartGame);
    }

    private void StartGame(List<Player> players)
    {
        StartCoroutine(StartGameAsync(players));
        
    }

    private IEnumerator StartGameAsync(List<Player> players)
    {
        playerSetupManager.enabled = false;
        playerSetupCanvas.SetActive(false);

        bikeManager.RemoveAllPlayers();

        yield return SceneManager.UnloadSceneAsync(practiceSceneIndex);
        yield return SceneManager.LoadSceneAsync(practiceSceneIndex, LoadSceneMode.Additive);

        var spawnLocater = FindObjectOfType<SpawnLocater>();
        bikeManager.Init(spawnLocater);

        liveGameManager.Init(bikeManager, players);
    }
}
