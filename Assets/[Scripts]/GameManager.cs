using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NONE,
    SCAN,
    EXTRACT
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    GameState gameState = GameState.SCAN;

    public int numScans;
    public int numExtracts;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TileClicked(HexTile tile)
    {
        switch(gameState)
        {
            case GameState.SCAN:
                ScanTile(tile);
                break;
            case GameState.EXTRACT:
                ExtractTile(tile);
                break;
            default:
                break;
        }
    }

    void ScanTile(HexTile passedtile)
    {
        passedtile.isScanned = true;
        passedtile.UpdateTile();
        foreach (GameObject hex in passedtile.neighbours)
        {
            HexTile tile = hex.GetComponent<HexTile>();
            tile.isScanned = true;
            tile.UpdateTile();
        }
    }

    void ExtractTile(HexTile passedtile)
    {

    }
}
