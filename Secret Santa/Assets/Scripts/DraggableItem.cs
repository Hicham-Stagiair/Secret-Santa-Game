using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public TMP_Text quantityText;
    public Item item;
    public Vector2 offset;
    public bool isInShop; // Flag to check if the item is in the shop

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public int quantity = 1;
    [HideInInspector] public bool isSelected;
    [HideInInspector] public bool planting = false;
    [HideInInspector] public bool watering;

    private Image image;
    private TMP_Text descriptionText;
    private GameObject plantButton;
    private GameObject gridParent;
    private bool setOnce = true;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = item.icon;
        gameObject.name = item.name;
        descriptionText = GameObject.Find("Description Text").GetComponent<TMP_Text>();
        plantButton = GameObject.Find("Plant Button Parent").transform.GetChild(0).gameObject;
        gridParent = GameObject.Find("Grid UI");
    }

    private void Update()
    {
        if (planting || watering)
        {
            if (parentAfterDrag == null)
            {
                parentAfterDrag = transform.parent;
                image.raycastTarget = false;
                setOnce = false;
            }

            transform.position = new Vector2(Input.mousePosition.x - offset.x, Input.mousePosition.y - offset.y);
        }
        else
        {
            if(parentAfterDrag != null && !setOnce)
            {
                transform.SetParent(parentAfterDrag);
                transform.position = parentAfterDrag.position;
                image.raycastTarget = true;
                parentAfterDrag = null;
                setOnce = true;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isInShop) return; // Prevent dragging if the item is in the shop

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isInShop) return; // Prevent dragging if the item is in the shop

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isInShop) return; // Prevent dragging if the item is in the shop

        transform.SetParent(parentAfterDrag);
        parentAfterDrag = null;
        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (Transform cell in gridParent.transform)
        {
            if (cell.childCount > 0)
            {
                cell.GetChild(0).GetComponent<DraggableItem>().isSelected = false;
            }
        }

        isSelected = true;

        if (isInShop)
        {
            item.isSelected = true;
            ShopManager.Instance.SelectItem(item);
            return;
        }

        descriptionText.text = item.description;

        if (item.type == ItemType.Seed)
        {
            plantButton.SetActive(true);
            plantButton.GetComponentInChildren<TMP_Text>().text = "Plant";

        }
        else
        {
            plantButton.SetActive(false);
        }

        if(item.type == ItemType.WateringCan)
        {
            plantButton.SetActive(true);
            plantButton.GetComponentInChildren<TMP_Text>().text = "Water";
        }
    }
}