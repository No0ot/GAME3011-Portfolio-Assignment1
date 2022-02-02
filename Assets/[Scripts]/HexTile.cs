//
// Based off code from https://www.redblobgames.com/grids/hexagons/
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexTile : MonoBehaviour
{
    public Vector3 coordinates;
	public Vector3[] directions = new Vector3[] {	new Vector3(0.0f, -1.0f, 1.0f), //
											new Vector3(1.0f, -1.0f, 0.0f), // 
											new Vector3(1.0f, 0.0f, -1.0f),	//
											new Vector3(0.0f, 1.0f, -1.0f),	//
											new Vector3(-1.0f, 1.0f, 0.0f),	//
											new Vector3(-1.0f, 0.0f, 1.0f)};//

	public List<GameObject> neighbours =  new List<GameObject>();

	public Color highValueColor;
	public Color midValueColor;
	public Color lowValueColor;
	public Color noValueColor;

	public int resourceValue;
	public bool isScanned = false;
	//References
	public GameObject selector;
	public SpriteRenderer tileBackground;


	//Layout and Orientation stuff
	static Orientation pointyOrientation = new Orientation( Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f,
															Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f,
															0.5f);
	public Layout hexLayout = new Layout(pointyOrientation, new Vector2(0.44f, 0.38f), new Vector2(0, 0));
    //Methods

    private void Start()
    {
		//neighbours = new List<GameObject>();
		//isScanned = true;
    }

    private void Update()
    {
		//UpdateTile();
    }
    public Vector2 hex_to_pixel( Vector3 h)
	{
		Orientation M = hexLayout.orientation;
		float x = (M.f0 * h.x + M.f1 * h.y) * hexLayout.size.x;
		float y = (M.f2 * h.x + M.f3 * h.y) * hexLayout.size.y;
		return new Vector2(x + hexLayout.origin.x, y + hexLayout.origin.y);
	}

    private void OnMouseEnter()
    {
		
		selector.SetActive(true);

	}
    private void OnMouseExit()
    {
		selector.SetActive(false);
	}

    private void OnMouseDown()
    {
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			GameManager.Instance.TileClicked(this);
		}
	}

	public void UpdateTile()
    {
		if (isScanned)
		{
			switch (resourceValue)
			{
				case int expression when resourceValue > 5000:
					tileBackground.color = highValueColor;
					break;
				case int expression when resourceValue > 2500 && resourceValue < 5000:
					tileBackground.color = midValueColor;
					break;
				case int expression when resourceValue > 1000 && resourceValue < 2500:
					tileBackground.color = lowValueColor;
					break;
				case int expression when resourceValue <= 1000:
					tileBackground.color = noValueColor;
					break;
				default:
					tileBackground.color = noValueColor;
					break;
			}
		}
	}

	public void SetResourceValues(int resourceamount)
    {
		resourceValue = resourceamount;
		foreach (GameObject hex in neighbours)
		{
			HexTile tile = hex.GetComponent<HexTile>();
			int halfvalue = resourceamount / 2;
			if (tile.resourceValue <= halfvalue)
				tile.resourceValue = halfvalue;

		}
		foreach (GameObject hex in neighbours)
		{
			HexTile tile = hex.GetComponent<HexTile>();
			foreach (GameObject hex2 in tile.neighbours)
			{
				HexTile tile2 = hex2.GetComponent<HexTile>();
				int quartervalue = resourceamount / 4;
				if (tile2.resourceValue <= quartervalue)
					tile2.resourceValue = quartervalue;
			}
		}
	}

	public void ExtractResource()
    {
		foreach (GameObject hex in neighbours)
		{
			HexTile tile = hex.GetComponent<HexTile>();
			tile.resourceValue = tile.resourceValue / 2;
			tile.UpdateTile();
			foreach (GameObject hex2 in tile.neighbours)
			{
				HexTile tile2 = hex2.GetComponent<HexTile>();
				int quartervalue = tile2.resourceValue / 2;
				tile2.resourceValue = quartervalue;
				tile2.UpdateTile();
			}
		}
		resourceValue = 0;
		UpdateTile();
	}		
}
