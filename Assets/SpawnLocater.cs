using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocater : MonoBehaviour
{
    [SerializeField] List<Transform> spawns;

    private int spawnIndex = 0;

    public Transform GetNextSpawn()
    {
        var returner = spawns[spawnIndex % spawns.Count];
        spawnIndex++;
        return returner;
    }
}
