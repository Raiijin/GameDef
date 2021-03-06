﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuScript : MonoBehaviour
{
    [MenuItem("Tools/Assign Tile Script")]
    public static void AssignTileScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach(GameObject t in tiles)
        {
            t.AddComponent<Tile>();
        }
    }

    [MenuItem("Tools/Clear Script")]
    public static void ClearScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject t in tiles)
        {
            DestroyImmediate(t.GetComponent<Tile>());
        }
    }
}
