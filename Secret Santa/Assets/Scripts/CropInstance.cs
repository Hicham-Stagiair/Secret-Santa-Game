using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class CropInstance : MonoBehaviour
{
    public Crop crop;
    public Item harvestItem;
    public SpriteRenderer spriteRenderer;

    public GameObject canvas;
    public Image progressBar; // Reference to the UI Image for the progress bar
    public Image waterBar; // Reference to the UI Image for the water bar

    public MMF_Player selectFeedback;

    private int currentStage = 0;
    private float growthTimer;
    private float hydrationTimer;
    private bool isSelected = false;

    public void Initialize(Crop crop)
    {
        this.crop = crop;
        harvestItem = crop.harvestItem;
        spriteRenderer.sprite = crop.growthStages[0];
        hydrationTimer = crop.hydrationTime;
        
        progressBar.fillAmount = 0f; // Initialize the progress bar
        waterBar.fillAmount = 1f; // Initialize the water bar
    }

    void Update()
    {
        if (IsFullyGrown())
        {
            GameObject growBar = progressBar.transform.parent.gameObject;
            GameObject waterBar = this.waterBar.transform.parent.gameObject;

            growBar.SetActive(false);
            waterBar.SetActive(false);
        }
        else
        {
            GrowCrop();
        }

        // Check for input to harvest the crop
        if (isSelected && Input.GetKeyDown(KeyCode.F))
        {
            if (IsFullyGrown())
            {
                InventoryManager.Instance.AddItemToInventory(harvestItem);
                Destroy(gameObject);
            }
        }
    }

    private void GrowCrop()
    {
        if (hydrationTimer > 0)
        {
            hydrationTimer -= Time.deltaTime;
            waterBar.fillAmount = hydrationTimer / crop.hydrationTime;

            growthTimer += Time.deltaTime;
            if (growthTimer >= crop.timeBetweenStages && currentStage < crop.growthStages.Length - 1)
            {
                growthTimer = 0f;
                currentStage++;
                spriteRenderer.sprite = crop.growthStages[currentStage];
            }

            // Update the progress bar fill amount
            progressBar.fillAmount = growthTimer / crop.timeBetweenStages;
        }
        else
        {
            // If the crop is not watered in time, it withers
            //spriteRenderer.sprite = crop.witheredStage;
        }
    }
    
    public void WaterCrop()
    {
        hydrationTimer = crop.hydrationTime;
        waterBar.fillAmount = 1f;
    }

    public bool IsFullyGrown()
    {
        return currentStage == crop.growthStages.Length - 1;
    }

    void OnMouseDown()
    {
        isSelected = true;
        canvas.SetActive(isSelected);
        selectFeedback.PlayFeedbacks();
    }

    void OnMouseExit()
    {
        isSelected = false;
        canvas.SetActive(isSelected);
    }
}