using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTracker
{
    private List<int> options = new List<int>();
    private bool collapsed;
    private Vector3Int coord;
    private TileBase selectedTile;
    private int tileValue;

    public TileTracker(List<int> options, bool collapsed, Vector3Int coord)
    {
        this.options = options;
        this.collapsed = collapsed;
        this.coord = coord;
    }

    public TileBase GetSelectedTile()
    {
        return selectedTile;
    }

    public void SetSelectedTile(TileBase selectedTile)
    {
        this.selectedTile = selectedTile;
    }

    public int GetEntropy()
    {
        return options.Count;
    }

    public List<int> GetOptions()
    {
        return options;
    }

    public void RemoveFromOptions(int value)
    {
        options.Remove(value);
    }

    public void SetOptions(List<int> options)
    {
        this.options = options;
    }

    public bool GetCollapsed()
    {
        return collapsed;
    }

    public void SetCollapsed(bool collapsed)
    {
        this.collapsed = collapsed;
    }

    public Vector3Int GetCoord()
    {
        return coord;
    }

    public int GetTileValue()
    {
        return tileValue;
    }

    public void SetTileValue(int tileValue)
    {
        this.tileValue = tileValue;
    }
}
