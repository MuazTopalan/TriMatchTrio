using UnityEngine;

[CreateAssetMenu(menuName = "Match3/Item")]
public class Item : ScriptableObject
{
    public int value;
    public Sprite sprite;
    public bool isSandblock; // Flag to identify if the item is a sandblock
    public int levelRequirement; // Level requirement to spawn this item
}
