using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLoader : MonoBehaviour
{
    [SerializeField] GameObject cameras1;
    [SerializeField] List<GameObject> cameras2;
    [SerializeField] List<GameObject> cameras4;
    [SerializeField] List<GameObject> cameras6;
    [SerializeField] List<GameObject> cameras9;

    public GameObject GetCameraPrefab(int playerIndex, int totalPlayerNumber)
    {
        switch (totalPlayerNumber)
        {
            case 1:
                return cameras1;
            case 2:
                return cameras2[playerIndex];
            case 3:
            case 4:
                return cameras4[playerIndex];
            case 5:
            case 6:
                return cameras6[playerIndex];
            case 7:
            case 8:
            case 9:
                return cameras9[playerIndex];
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
