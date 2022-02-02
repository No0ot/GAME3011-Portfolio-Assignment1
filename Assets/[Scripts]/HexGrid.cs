//
// Based off code from https://www.redblobgames.com/grids/hexagons/
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public HexTile hexPrefab;
    public int mapRadius;

    List<GameObject> hexList;

    public int resourceValueMin;
    public int resourceValueMax;

    public int numHighValueResourceNodes;
    // Start is called before the first frame update
    void Awake()
    {
        hexList = new List<GameObject>();
        BuildGrid();
        MapGrid();
    }

    private void OnEnable()
    {
        AddResourcesToTiles();
        
    }

    private void OnDisable()
    {
        ResetGrid();
    }

    void BuildGrid()
    {
        for (int q = -mapRadius; q <= mapRadius; q++)
        {
            int r1 = Mathf.Max(-mapRadius, -q - mapRadius);
            int r2 = Mathf.Min(mapRadius, -q + mapRadius);
            for (int r = r1; r <= r2; r++)
            {
                HexTile temp = Instantiate(hexPrefab, this.transform);
                temp.coordinates = new Vector3(q, r, -q - r);
                hexList.Add(temp.gameObject);
                Vector2 position = temp.hex_to_pixel(temp.coordinates);
                temp.gameObject.transform.position = new Vector3(position.x, position.y, 0.0f);
            }
        }
    }

    void MapGrid()
    {
        foreach (GameObject hex in hexList)
        {
            HexTile tile = hex.GetComponent<HexTile>();
            foreach(Vector3 neighbour in tile.directions)
            {
                float x = tile.coordinates.x + neighbour.x;
                float y = tile.coordinates.y + neighbour.y;
                float z = tile.coordinates.z + neighbour.z;

                if (x < mapRadius + 1.0f && x > -mapRadius - 1.0f && y < mapRadius + 1.0f && y > -mapRadius - 1.0f && z < mapRadius + 1.0f && z > -mapRadius - 1.0f)
                {
                    GameObject temp = returnHex(new Vector3(x, y, z));
                    tile.neighbours.Add(temp);
                }
            }
        }
    }

    void AddResourcesToTiles()
    {
        for(int i = 0; i < numHighValueResourceNodes; i++)
        {
            int index = Random.Range(0, hexList.Count);
            int value = Random.Range(resourceValueMin, resourceValueMax);

            HexTile tile = hexList[index].GetComponent<HexTile>();
            tile.SetResourceValues(value);
        }
    }

    GameObject returnHex(Vector3 search)
    {
        GameObject temp = null;

        foreach (GameObject hex in hexList)
        {
            HexTile tile = hex.GetComponent<HexTile>();
            if (tile.coordinates == search)
            {
                temp = hex;
            }
        }

        return temp;
    }

    void ResetGrid()
    {
        foreach (GameObject hex in hexList)
        {
            HexTile tile = hex.GetComponent<HexTile>();
            tile.resourceValue = 0;
            tile.UpdateTile();
            tile.isScanned = false;
        }
    }
}
