using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "SO/Item")]
public class Item : ScriptableObject
{
    public ItemType type;
    public string name;
    public Sprite icon;
    public int value;
    public int maxStackSize;
    public string description;
    public Crop crop;
    public bool isSelected;
}

public enum ItemType
{
    Seed,
    Crop,
    WateringCan
}
