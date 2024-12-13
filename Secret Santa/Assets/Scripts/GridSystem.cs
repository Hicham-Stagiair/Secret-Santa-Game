using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase defaultTile;
    public TileBase selectedTile;

    void Start()
    {
        // Initialize the tilemap with default tiles if needed
    }

    public Vector3Int GetGridPosition(Vector3 worldPosition)
    {
        return tilemap.WorldToCell(worldPosition);
    }

    public void SetTile(Vector3Int gridPosition, TileBase tile)
    {
        tilemap.SetTile(gridPosition, tile);
    }

    public void SpawnObjectAtGridPosition(Vector3Int gridPosition, GameObject prefab)
    {
        Vector3 worldPosition = tilemap.CellToWorld(gridPosition) + tilemap.cellSize / 2;
        Instantiate(prefab, worldPosition, Quaternion.identity);
    }
}