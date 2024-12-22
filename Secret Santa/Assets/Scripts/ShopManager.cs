using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public GameObject shop;
    public GameObject itemPrefab;
    public GameObject sellGrid;
    public GameObject gridSystem;
    public GameObject stopPlantingButton;
    public TMP_Text itemCostText;
    public TMP_Text quantityText;
    public GameObject inventory;
    public GameObject sellInventory;

    private Item selectedItem;
    private int selectedQuantity = 1;

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

    // Selects an item and updates the UI to reflect the selected item and quantity
    public void SelectItem(Item item)
    {
        selectedItem = item;
        selectedQuantity = 1;
        UpdateUI();
    }

    public void IncreaseQuantity()
    {
        selectedQuantity++;
        UpdateUI();
    }

    public void DecreaseQuantity()
    {
        if (selectedQuantity > 1)
        {
            selectedQuantity--;
            UpdateUI();
        }
    }

    // Buys the selected item if the player has enough money, otherwise logs a message
    public void BuyItem()
    {
        int totalCost = selectedItem.value * selectedQuantity;
        if (PlayerWallet.Instance.money >= totalCost)
        {
            PlayerWallet.Instance.RemoveMoney(totalCost);
            AddItemToInventory(selectedItem, selectedQuantity);
        }
        else
        {
            Debug.Log("Not enough money to buy the item.");
        }
    }
    
    public void SellItem()
    {
        foreach (Transform slot in sellInventory.transform)
        {
            if (slot.childCount > 0)
            {
                GameObject itemInstance = slot.GetChild(0).gameObject;
                DraggableItem draggableItem = itemInstance.GetComponent<DraggableItem>();
                int totalValue = draggableItem.item.value * draggableItem.quantity;
                PlayerWallet.Instance.AddMoney(totalValue);
                Destroy(itemInstance);
            }
        }
    }

    private void UpdateUI()
    {
        itemCostText.text = (selectedItem.value * selectedQuantity).ToString();
        quantityText.text = selectedQuantity.ToString();
    }

    // Adds the purchased item to the inventory, either creating a new slot or updating an existing one
    private void AddItemToInventory(Item item, int quantity)
    {
        foreach (Transform slot in inventory.transform)
        {
            if (slot.childCount == 0)
            {
                GameObject itemInstance = Instantiate(itemPrefab, slot);
                DraggableItem draggableItem = itemInstance.GetComponent<DraggableItem>();
                draggableItem.parentAfterDrag = slot;
                draggableItem.item = item;
                draggableItem.quantity = quantity;
                draggableItem.quantityText.text = quantity.ToString();
                break;
            }
            else
            {
                GameObject itemInstance = slot.GetChild(0).gameObject;
                DraggableItem draggableItem = itemInstance.GetComponent<DraggableItem>();
                if (draggableItem.item == item)
                {
                    draggableItem.quantity += quantity;
                    draggableItem.quantityText.text = draggableItem.quantity.ToString();
                    break;
                }
            }
        }
    }
}