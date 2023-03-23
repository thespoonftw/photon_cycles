
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraValues
{
    private static Rect cameras1 = new Rect(0, 0, 1, 1);

    private static List<Rect> cameras2 = new()
    {
        new Rect(0.15f, 0.5f, 0.7f, 0.5f),
        new Rect(0.15f, 0f,   0.7f, 0.5f)
    };

    private static List<Rect> cameras4 = new()
    {
        new Rect(0f,   0.5f, 0.5f, 0.5f),
        new Rect(0f,   0f,   0.5f, 0.5f),
        new Rect(0.5f, 0.5f, 0.5f, 0.5f),
        new Rect(0.5f, 0f,   0.5f, 0.5f)
    };

    private static List<Rect> cameras6 = new()
    {
        new Rect(0.05f, 0.666f, 0.45f, 0.333f),
        new Rect(0.05f, 0.333f, 0.45f, 0.333f),
        new Rect(0.5f,  0.666f, 0.45f, 0.333f),
        new Rect(0.5f,  0.333f, 0.45f, 0.333f),
        new Rect(0.05f, 0f,     0.45f, 0.333f),
        new Rect(0.5f,  0f,     0.45f, 0.333f)
    };

    private static List<Rect> cameras9 = new()
    {
        new Rect(0f,     0.666f, 0.333f, 0.333f),
        new Rect(0f,     0.333f, 0.333f, 0.333f),
        new Rect(0.333f, 0.666f, 0.333f, 0.333f),
        new Rect(0.333f, 0.333f, 0.333f, 0.333f),
        new Rect(0f,     0f,     0.333f, 0.333f),
        new Rect(0.333f, 0f,     0.333f, 0.333f),
        new Rect(0.666f, 0.666f, 0.333f, 0.333f),
        new Rect(0.666f, 0.333f, 0.333f, 0.333f),
        new Rect(0.666f, 0f,     0.333f, 0.333f)
    };

    public static Rect GetCameraRect(int playerIndex, int totalPlayerNumber)
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
