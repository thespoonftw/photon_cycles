using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocater : MonoBehaviour
{
    [SerializeField] List<Transform> spawns;

    public Transform GetSpawnForPlayer(int playerIndex)
    {
        return spawns[playerIndex];
    }
}
