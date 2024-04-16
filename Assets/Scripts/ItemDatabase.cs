using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static Item[] Items { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // Load all items from the Resources folder
        Items = Resources.LoadAll<Item>("Items/");

        foreach (Item item in Items)
        {
            switch (item.name)
            {
                case "Sandblock":
                    item.levelRequirement = 3; // Sandblock için level gereksinimi 3
                    item.isSandblock = true;
                    break;
                default:
                    item.levelRequirement = 1; // Diğer item'lar için varsayılan level gereksinimi 1
                    item.isSandblock = false;
                    break;
            }
        }

        // Set level requirement for each item
        // foreach (Item item in Items)
        // {
        //     switch (item.name) // Assuming the name of the item corresponds to its level requirement
        //     {
        //         case "Item1":
        //         case "Item2":
        //         case "Item3":
        //         case "Item4":
        //         case "Item5":
        //         case "Item6":
        //             item.levelRequirement = 1; // 1. seviyeden itibaren
        //             item.isSandblock = false; // Sandblock değil
        //             break;
        //         case "Sandblock":
        //             item.levelRequirement = 3; // 3. seviyeden itibaren
        //             item.isSandblock = true; // Sandblock
        //             break;
        //         default:
        //             item.levelRequirement = 1; // Default olarak 1. seviyeden itibaren
        //             item.isSandblock = false; // Sandblock değil
        //             break;
        //     }
        // }
    }
}
