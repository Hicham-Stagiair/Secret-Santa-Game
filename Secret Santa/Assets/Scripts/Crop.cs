using UnityEngine;

[CreateAssetMenu(  fileName = "New Crop", menuName = "SO/Crop")]
public class Crop : ScriptableObject
{
    public string cropName;
    public int timeBetweenStages;
    public int hydrationTime;
    public Sprite[] growthStages;
    public Sprite witheredStage;
    public Item harvestItem;
}
