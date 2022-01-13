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

    public GameState gameState = GameState.SCAN;

    public int numScans;
    public int numExtracts;
    public int score;

    bool scanToggle;
    bool extractToggle;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        numScans = 6;
        numExtracts = 3;
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
        if (numScans > 0)
        {
            passedtile.isScanned = true;
            passedtile.UpdateTile();
            foreach (GameObject hex in passedtile.neighbours)
            {
                HexTile tile = hex.GetComponent<HexTile>();
                tile.isScanned = true;
                tile.UpdateTile();
            }
            numScans--;
        }
    }

    void ExtractTile(HexTile passedtile)
    {
        if(passedtile.isScanned)
        {
            if (numExtracts > 0)
            {
                score += passedtile.resourceValue;
                passedtile.ExtractResource();
                numExtracts--;
            }
        }
    }

    public void ScanToggle(bool tf)
    {
        scanToggle = tf;
        if (scanToggle)
            gameState = GameState.SCAN;
        else
            gameState = GameState.NONE;
    }

    public void ExtractToggle(bool tf)
    {
        extractToggle = tf;
        if (extractToggle)
            gameState = GameState.EXTRACT;
        else
            gameState = GameState.NONE;
    }
}
