using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaveFunctionCollapse : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;

    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase[] tiles;
    [SerializeField] private List<Tiles> tileInfo;
    private const int dim = 10;
    private TileTracker[,] tileData = new TileTracker[dim, dim];
    private int[] defaultOptions = { 0, 1, 2, 3, 4 };
    private int numCollapsed;

    void Start()
    {
        cameraPos.position = new Vector3(dim / 2, dim / 2, -10f);

        for (int i = 0; i < dim; i++)
        {
            for(int j = 0; j < dim; j++)
            {
                TileTracker tt = new TileTracker(defaultOptions.ToList(), false, new Vector3Int(i, j, 0));
                tileData[i, j] = tt;
            }
        }

        TileTracker firstCollapsed = tileData[UnityEngine.Random.Range(0, dim), UnityEngine.Random.Range(0, dim)];
        int randomTileValue = UnityEngine.Random.Range(0, defaultOptions.Length);
        firstCollapsed.SetSelectedTile(tiles[randomTileValue]);
        firstCollapsed.SetCollapsed(true);
        firstCollapsed.SetTileValue(randomTileValue);
        PropogateEntropy(firstCollapsed);

        while (numCollapsed < dim * dim - 1)
        {
            List<TileTracker> lowestEntropyTiles = FindLowestEntropyTiles();
            TileTracker currTile = lowestEntropyTiles[UnityEngine.Random.Range(0, lowestEntropyTiles.Count)];
            randomTileValue = UnityEngine.Random.Range(0, currTile.GetOptions().Count);
            currTile.SetSelectedTile(tiles[currTile.GetOptions()[randomTileValue]]);
            currTile.SetCollapsed(true);
            currTile.SetTileValue(currTile.GetOptions()[randomTileValue]);
            PropogateEntropy(currTile);
            numCollapsed++;
        }


        //Display Tiles --> DONE LAST
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if (tileData[i, j].GetCollapsed())
                {
                    tileMap.SetTile(new Vector3Int(i, j, 0), tileData[i, j].GetSelectedTile());
                }
            }
        }
    }

    List<TileTracker> FindLowestEntropyTiles()
    {
        List<TileTracker> lowestEntropyTiles = new List<TileTracker>();
        int lowestEntropy = 999;

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if (tileData[i, j].GetEntropy() < lowestEntropy && !tileData[i, j].GetCollapsed())
                {
                    lowestEntropy = tileData[i, j].GetEntropy();
                }
            }
        }

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                if (tileData[i, j].GetEntropy() == lowestEntropy && !tileData[i, j].GetCollapsed())
                {
                    lowestEntropyTiles.Add(tileData[i, j]);
                }
            }
        }

        return lowestEntropyTiles;
    }

    void PropogateEntropy(TileTracker collapsedTile)
    {
        if (collapsedTile.GetCoord().y + 1 < dim)
        {
            tileData[collapsedTile.GetCoord().x, collapsedTile.GetCoord().y + 1].SetOptions(CompareOptions(tileInfo[collapsedTile.GetTileValue()].upTiles, tileData[collapsedTile.GetCoord().x, collapsedTile.GetCoord().y + 1].GetOptions()));
        }

        if (collapsedTile.GetCoord().x + 1 < dim)
        {
            tileData[collapsedTile.GetCoord().x + 1, collapsedTile.GetCoord().y].SetOptions(CompareOptions(tileInfo[collapsedTile.GetTileValue()].rightTiles, tileData[collapsedTile.GetCoord().x + 1, collapsedTile.GetCoord().y].GetOptions()));
        }

        if (collapsedTile.GetCoord().y - 1 >= 0)
        {
            tileData[collapsedTile.GetCoord().x, collapsedTile.GetCoord().y - 1].SetOptions(CompareOptions(tileInfo[collapsedTile.GetTileValue()].downTiles, tileData[collapsedTile.GetCoord().x, collapsedTile.GetCoord().y - 1].GetOptions()));
        }

        if (collapsedTile.GetCoord().x - 1 >= 0)
        {
            tileData[collapsedTile.GetCoord().x - 1, collapsedTile.GetCoord().y].SetOptions(CompareOptions(tileInfo[collapsedTile.GetTileValue()].leftTiles, tileData[collapsedTile.GetCoord().x - 1, collapsedTile.GetCoord().y].GetOptions()));
        }

    }

    List<int> CompareOptions(int[] directionalOptions, List<int> neighbourOptions)
    {
        List<int> newNeighbourOptions = new List<int>();

        foreach (int option in neighbourOptions)
        {
            newNeighbourOptions.Add(option);
        }

        for (int i = 0; i < neighbourOptions.Count; i++)
        {
            if (Array.IndexOf(directionalOptions, neighbourOptions[i]) == -1)
            {
                newNeighbourOptions.Remove(neighbourOptions[i]);
            }
        }

        return newNeighbourOptions;
    }
}
