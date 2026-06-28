using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Unity.VisualScripting;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance {get; private set; }

    [SerializeField] private Tilemap farmTilemap;
    [SerializeField] private TileBase tilledDryTile;
    [SerializeField] private TileBase tilledWateredTile;

    // The data model - which cells are tilled
    private Dictionary<Vector3Int, TileState> tiles = new Dictionary<Vector3Int, TileState>();

    void Awake()
    {
        Instance = this;
    }

    public void HoeTile(Vector3Int cell)
    {
        var state = GetOrCreate(cell);
        if (state.isTilled) return;

        state.isTilled = true;
        RefreshVisual(cell);
    }

    public void WaterTile(Vector3Int cell)
    {
        var state = GetTile(cell);
        // only water tilled soil
        if (state == null || !state.isTilled || state.isWatered) return;

        state.isWatered = true;
        RefreshVisual(cell);
    }
    // Player action - Untill a hoed tile but not a watered one
    public void TryUntillTile(Vector3Int cell)
    {
        var state = GetTile(cell);
        if (state == null || !state.isTilled) return;
        if (state.isWatered) return;

        state.isTilled = false;
        RefreshVisual(cell);
    }
    // Removes entry from the dictionary instead of leaving a dead TileState in memory
    public void ResetTile(Vector3Int cell)
    {
        if (!tiles.ContainsKey(cell)) return;

        tiles.Remove(cell);
        RefreshVisual(cell);
    }

    public bool IsTilled(Vector3Int cell) => GetTile(cell)?.isTilled ?? false;
    public bool IsWatered(Vector3Int cell) => GetTile(cell)?.isWatered ?? false;

// -- Internals --
    private TileState GetTile(Vector3Int cell)
    {
        tiles.TryGetValue(cell,out var state);
        return state;
    }

    private TileState GetOrCrate(Vector3Int cell)
    {
        if(!tiles.TryGetValue(cell, out var state))
        {
                    state = new TileState();
        tiles[cell] = state;
        }
        return state;
    }

    private TileState GetOrCreate(Vector3Int cell)
    {
        if (!tiles.TryGetValue(cell, out var state))
        {
            state = new TileState();
            tiles[cell] = state;
        }
        return state;
    }

    private void RefreshVisual(Vector3Int cell)
    {
        var state = GetTile(cell);
        if (state == null || !state.isTilled)
        {
            farmTilemap.SetTile(cell,null);
            return;
        }
        farmTilemap.SetTile(cell,
            state.isWatered ? tilledWateredTile : tilledDryTile);
        
    }
}

public class TileState
{
    public bool isTilled;
    public bool isWatered;
}