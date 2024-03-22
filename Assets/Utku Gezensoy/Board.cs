using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    public Row[] rows;
    public Tile[,] Tiles { get; private set; }

    public int Width => Tiles.GetLength( dimension: 0);
    public int Height => Tiles.GetLength(dimension: 1);

    private void Awake() => Instance = this;

    private void Start()
    {
        Tiles = new Tile[rows.Max(selector:row => row.tiles.Length), rows.Length];

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = rows[y].tiles[x]; 

                tile.x = x;
                tile.y = y;
                
                tile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
                Tiles[x,y] = tile;
            }
        }
    }
}
 