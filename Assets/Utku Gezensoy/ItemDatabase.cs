using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static Item[] Items { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // Load all items from the Resources folder
        Items = Resources.LoadAll<Item>("Items/");
    }
}
