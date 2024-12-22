using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase selectedTile;

    public void SetTile(Vector3Int gridPosition, TileBase tile)
    {
        tilemap.SetTile(gridPosition, tile);
    }

    public void SpawnObjectAtGridPosition(Vector3Int gridPosition, GameObject objectPrefab, Crop crop)
    {
        if (!IsCellOccupied(gridPosition))
        {
            Vector3 worldPosition = tilemap.CellToWorld(gridPosition) + tilemap.cellSize / 2;
            GameObject spawnCrop = Instantiate(objectPrefab, worldPosition, Quaternion.identity);
            spawnCrop.GetComponent<CropInstance>().Initialize(crop);
            
        }
    }
    
    public CropInstance GetCropInstance(Vector3Int gridPosition)
    {
        if (IsCellOccupied(gridPosition))
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(tilemap.CellToWorld(gridPosition) + tilemap.cellSize / 2, tilemap.cellSize * 0.9f, 0);
            foreach (var collider in colliders)
            {
                CropInstance cropInstance = collider.GetComponent<CropInstance>();
                if (cropInstance != null)
                {
                    return cropInstance;
                }
            }
        }
        return null;
    }

    public Vector3Int GetGridPosition(Vector3 worldPosition)
    {
        return tilemap.WorldToCell(worldPosition);
    }

    public bool IsCellOccupied(Vector3Int gridPosition)
    {
        Vector3 cellCenter = tilemap.CellToWorld(gridPosition) + tilemap.cellSize / 2;
        Vector2 cellSize = tilemap.cellSize * 0.9f; // Reduce the size slightly to avoid overlapping multiple cells
        Collider2D[] colliders = Physics2D.OverlapBoxAll(cellCenter, cellSize, 0);
        return colliders.Length > 0;
    }
}