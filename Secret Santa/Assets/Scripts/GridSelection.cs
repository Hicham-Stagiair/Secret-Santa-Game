using UnityEngine;

public class GridSelection : MonoBehaviour
{
    public GridSystem gridSystem;
    public Camera mainCamera;
    public GameObject objectPrefab;
    public Sprite highlightSprite; // Reference to the highlight sprite

    private GameObject highlightObject;
    private Vector3 mousePosition;
    private Vector3Int gridPosition;

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
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            gridPosition = gridSystem.GetGridPosition(mousePosition);

            // Update the position of the highlight object
            highlightObject.transform.position = gridSystem.tilemap.CellToWorld(gridPosition) + gridSystem.tilemap.cellSize / 2;
            highlightObject.SetActive(true); // Show the highlight
        }

        if (Input.GetKey(KeyCode.R))
        {
            gridSystem.SetTile(gridPosition, gridSystem.selectedTile);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            gridSystem.SpawnObjectAtGridPosition(gridPosition, objectPrefab);
        }
    }
}