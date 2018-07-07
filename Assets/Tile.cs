using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public static List<Tile> allTiles = new List<Tile>();
    private void Awake()
    {
        if (!allTiles.Contains(this))
        {
            allTiles.Add(this);
        }
    }
}
