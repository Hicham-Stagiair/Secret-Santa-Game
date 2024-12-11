using UnityEngine;
using UnityEngine.UI;

public class CropInstance : MonoBehaviour
{
    public Crop crop;
    public SpriteRenderer spriteRenderer;
    public Image progressBar; // Reference to the UI Image for the progress bar

    private int currentStage = 0;
    private float timer;

    void Start()
    {
        spriteRenderer.sprite = crop.growthStages[currentStage];
        progressBar.fillAmount = 0f; // Initialize the progress bar
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= crop.timeBetweenStages && currentStage < crop.growthStages.Length - 1)
        {
            timer = 0f;
            currentStage++;
            spriteRenderer.sprite = crop.growthStages[currentStage];
        }

        // Update the progress bar fill amount
        progressBar.fillAmount = timer / crop.timeBetweenStages;
    }

    public bool IsFullyGrown()
    {
        return currentStage == crop.growthStages.Length - 1;
    }
}