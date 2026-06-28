using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance {get; private set; }

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase tilledTile;

    // The data model - which cells are tilled
    private HashSet<Vector3Int> tilledCells = new HashSet<Vector3Int>();

    void Awake()
    {
        Instance = this;
    }

    public void HoeTile(Vector3Int cell)
    {
        if (tilledCells.Contains(cell)) return; // already tilled
        tilledCells.Add(cell);
        groundTilemap.SetTile(cell, tilledTile);
    }

    public bool IsTilled(Vector3Int cell) => tilledCells.Contains(cell);
}
