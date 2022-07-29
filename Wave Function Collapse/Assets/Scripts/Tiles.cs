using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Tiles : ScriptableObject
{
    //blank = 0, up = 1, right = 2, down = 3, left = 4
    public TileBase sprite;
    public int tileValue;
    public int[] upTiles;
    public int[] rightTiles;
    public int[] downTiles;
    public int[] leftTiles;
}
