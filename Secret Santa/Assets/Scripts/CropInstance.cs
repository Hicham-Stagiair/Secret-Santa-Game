using UnityEngine;
using UnityEngine.UI;

public class CropInstance : MonoBehaviour
{
    public Crop crop;
    public SpriteRenderer spriteRenderer;
    public Image progressBar; // Reference to the UI Image for the progress bar
    public Image waterBar; // Reference to the UI Image for the water bar

    private int currentStage = 0;
    private float growthTimer;
    private float hydrationTimer;

    void Start()
    {
        spriteRenderer.sprite = crop.growthStages[currentStage];
        progressBar.fillAmount = 0f; // Initialize the progress bar
        waterBar.fillAmount = 1f; // Initialize the water bar
        hydrationTimer = crop.hydrationTime;
    }

    void Update()
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
    }

    public bool IsFullyGrown()
    {
        return currentStage == crop.growthStages.Length - 1;
    }
}