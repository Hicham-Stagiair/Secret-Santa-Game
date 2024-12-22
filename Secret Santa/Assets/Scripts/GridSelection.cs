using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSelection : MonoBehaviour
{
    [HideInInspector] public bool canHighlight = false;
    public GridSystem gridSystem;
    public Camera mainCamera;
    public GameObject cropPrefab;
    public Sprite highlightSprite; // Reference to the highlight sprite
    public Tilemap targetTilemap; // Reference to the specific tilemap

    private GameObject highlightObject;
    private Vector3 mousePosition;
    private Vector3Int gridPosition;

    private Crop selectedCrop;
    private DraggableItem selectedSeed;
    
    private ItemType currentItemType;

    void Start()
    {
        // Create a GameObject for the highlight and set its sprite
        highlightObject = new GameObject("Highlight");
        SpriteRenderer spriteRenderer = highlightObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = highlightSprite;
        highlightObject.SetActive(false); // Hide the highlight initially
    }

    void Update()
    {
        if (canHighlight)
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            gridPosition = gridSystem.GetGridPosition(mousePosition);

            // Check if the tile at the grid position belongs to the target tilemap
            if (targetTilemap.HasTile(gridPosition))
            {
                // Update the position of the highlight object
                highlightObject.transform.position = targetTilemap.CellToWorld(gridPosition) + targetTilemap.cellSize / 2;
                highlightObject.SetActive(true); // Show the highlight

                if (Input.GetMouseButtonDown(0))
                {
                    if (currentItemType == ItemType.Seed)
                    {
                        if(gridSystem.IsCellOccupied(gridPosition)) return;
                        gridSystem.SpawnObjectAtGridPosition(gridPosition, cropPrefab, selectedCrop);
                        ReduceSeedQuantity();
                    }
                    else if (currentItemType == ItemType.WateringCan)
                    {
                        CropInstance cropInstance = gridSystem.GetCropInstance(gridPosition);
                        if (cropInstance != null)
                        {
                            Debug.Log("Watering crop");
                            cropInstance.WaterCrop();
                            ReduceSeedQuantity();
                        }
                    }
                }
            }
            else
            {
                highlightObject.SetActive(false); // Hide the highlight if the tile is not in the target tilemap
            }
        }
        else
        {
            highlightObject.SetActive(false); // Hide the highlight
        }
    }

    public void HighlightItem(Crop crop, DraggableItem seed, ItemType itemType)
    {
        canHighlight = true;
        currentItemType = itemType;
        selectedCrop = crop;
        selectedSeed = seed;
    }

    private void ReduceSeedQuantity()
    {
        if (selectedSeed != null)
        {
            selectedSeed.quantity--;
            if (selectedSeed.quantity <= 0)
            {
                Destroy(selectedSeed.gameObject);
                InventoryManager.Instance.CancelSelection();
            }
            else
            {
                selectedSeed.quantityText.text = selectedSeed.quantity.ToString();
            }
        }
    }
}