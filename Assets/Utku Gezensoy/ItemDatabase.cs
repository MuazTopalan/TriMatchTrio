using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static Item[] Items { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // Load all items from the Resources folder
        Items = Resources.LoadAll<Item>("Items/");

        // Set level requirement for each item
        foreach (Item item in Items)
        {
            switch (item.name) // Assuming the name of the item corresponds to its level requirement
            {
                case "Item1":
                case "Item2":
                    item.levelRequirement = 1; // 1. seviyeden itibaren
                    break;
                case "SandblockItem":
                case "Item3":
                case "Item4":
                case "Item5":
                    item.levelRequirement = 3; // 3. seviyeden itibaren
                    break;
                default:
                    item.levelRequirement = 1; // Default olarak 1. seviyeden itibaren
                    break;
            }
        }
    }
}
