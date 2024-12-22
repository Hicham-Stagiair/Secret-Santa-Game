using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public GameObject inventory;
    public GameObject itemPrefab;
    public GameObject gridSystem;
    public GameObject stopPlantingButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItemToInventory(Item item)
    {
        foreach (Transform slot in inventory.transform)
        {
            if (slot.childCount == 0)
            {
                GameObject itemInstance = Instantiate(itemPrefab, slot);
                itemInstance.GetComponent<DraggableItem>().parentAfterDrag = slot;
                itemInstance.GetComponent<DraggableItem>().item = item;
                break;
            }
            else
            {
                GameObject itemInstance = slot.GetChild(0).gameObject;
                if (itemInstance.GetComponent<DraggableItem>().item == item)
                {
                    if(itemInstance.GetComponent<DraggableItem>().quantity <= item.maxStackSize - 1)
                    {
                        itemInstance.GetComponent<DraggableItem>().quantity++;
                        itemInstance.GetComponent<DraggableItem>().quantityText.text = itemInstance.GetComponent<DraggableItem>().quantity.ToString();
                        break;
                    }
                }
            }
        }
    }
    
    public void RemoveItemFromInventory(Item item)
    {
        foreach (Transform slot in inventory.transform)
        {
            if (slot.childCount > 0)
            {
                GameObject itemInstance = slot.GetChild(0).gameObject;
                if (itemInstance.GetComponent<DraggableItem>().item == item)
                {
                    if (itemInstance.GetComponent<DraggableItem>().quantity > 1)
                    {
                        itemInstance.GetComponent<DraggableItem>().quantity--;
                        itemInstance.GetComponent<DraggableItem>().quantityText.text = itemInstance.GetComponent<DraggableItem>().quantity.ToString();
                    }
                    else
                    {
                        Destroy(itemInstance);
                    }
                    break;
                }
            }
        }
    }
    
    public void PlantCrop()
    {
        foreach (Transform slot in inventory.transform)
        {
            if (slot.childCount > 0)
            {
                GameObject itemInstance = slot.GetChild(0).gameObject;
                DraggableItem item = itemInstance.GetComponent<DraggableItem>();
                if (item.isSelected)
                {
                    if (item.item.type == ItemType.Seed)
                    {
                        item.planting = true;
                        Crop crop = item.item.crop;
                        gridSystem.GetComponent<GridSelection>().HighlightItem(crop, item, ItemType.Seed);
                    }
                    else if (item.item.type == ItemType.WateringCan)
                    {
                        item.watering = true;
                        gridSystem.GetComponent<GridSelection>().HighlightItem(null, item, ItemType.WateringCan);
                    }
                }
            }
        }
    }
    
    public void CancelSelection()
    {
        foreach (Transform slot in inventory.transform)
        {
            if (slot.childCount > 0)
            {
                GameObject itemInstance = slot.GetChild(0).gameObject;
                DraggableItem item = itemInstance.GetComponent<DraggableItem>();
                if (item.planting || item.watering)
                {
                    item.planting = false;
                    item.watering = false;
                    item.isSelected = false;
                    gridSystem.GetComponent<GridSelection>().canHighlight = false;
                }
            }
        }
        stopPlantingButton.SetActive(false);
    }
}