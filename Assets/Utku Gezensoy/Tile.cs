using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;

public class Tile : MonoBehaviour
{
    // Coordinates of the tile in the grid
    public int x;
    public int y;

    // Item associated with the tile
    private Item _item;

    // Property to access and set the item of the tile
    public Item Item
    {
        get => _item;
        set
        {
            if (_item == value) return;
            _item = value;
            icon.sprite = _item.sprite;
        }
    }

    // Reference to the UI image & Button
    public Image icon;
    public Button button;

    // Getters for neighboring tiles
    public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null;
    public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null;
    public Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null;
    public Tile Bottom => y < Board.Instance.Width - 1 ? Board.Instance.Tiles[x, y + 1] : null;

    // Array of neighboring tiles
    public Tile[] Neighbours => new[]
    {
        Left,
        Top,
        Right,
        Bottom,
    };

    // Called when the tile object is initialized
    private void Start()
    {
        // Add listener to the button click event
        button.onClick.AddListener(() => Board.Instance.Select(tile: this));
    }

    // Method to get connected tiles based on the same item
    public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    {
        var result = new List<Tile> { this, };

        if (exclude == null)
        {
            exclude = new List<Tile> { this, };
        }
        else
        {
            exclude.Add(this);
        }

        foreach (var neighbour in Neighbours)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item) continue;
            if (neighbour.Item.isSandblock) continue; // Skip sandblocks

            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }

        return result;
    }
}
