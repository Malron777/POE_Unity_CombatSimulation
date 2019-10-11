using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
  public List<GameObject> redUnits;
  public List<GameObject> greenUnits;
  public List<GameObject> grayUnits;
  public GameObject blankTile;

  public enum Direction { Up, Down, Left, Right };
  public int gridSizeX = 20;
  public int gridSizeY = 20;
  private GameObject[,] map;

  public float tickerTimeStart = 3f;
  public float tickerTime = 3f;

  // Start is called before the first frame update
  void Start()
  {
    //RedUnits = new List<GameObject>();
    //GreenUnits = new List<GameObject>();
    map = new GameObject[gridSizeX, gridSizeY];

    for (int i = 0; i < gridSizeX; i++)
    {
      for (int j = 0; j < gridSizeY; j++)
      {
        map[i, j] = blankTile;
        float xPos = (float)(0.16 + (i * 0.32));
        float yPos = (float)(0.16 + (j * 0.32));
        Vector2 position = new Vector2(xPos, yPos);
        Instantiate(map[i, j], position, Quaternion.identity);
      }
    }
  }

  // Update is called once per frame
  void Update()
  {    
    if (tickerTime <= 0)
    {
      for (int i = 0; i < gridSizeX; i++)
      {
        for (int j = 0; j < gridSizeY; j++)
        {
         Unit unit = map[i, j].GetComponent<Unit>();
          unit.FindClosestEnemy(map);
        }
      }
      tickerTime = tickerTimeStart;
    }
    else
    {
      tickerTime -= Time.deltaTime;
    }
  }
}
