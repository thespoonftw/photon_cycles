using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    IEnumerator Start()
    {
        SceneManager.LoadScene("PlayerSetup", LoadSceneMode.Additive);
        yield return null;
        var setup = FindObjectOfType<PlayerSetupManager>();

        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        yield return null;
        var spawnLocater = FindObjectOfType<SpawnLocater>();

        setup.Init(spawnLocater);
    }
}
