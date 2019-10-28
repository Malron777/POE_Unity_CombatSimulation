using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuilding : MonoBehaviour
{
  public Map map;

  private int _xPosition = 0;
  public int xPosition
  {
    get { return _xPosition; }
    set
    {
      if (value < 0)
      {
        _xPosition = 0;
      }
      else if (value >= map.gridSizeX)
      {
        _xPosition = map.gridSizeX - 1;
      }
      else
      {
        _xPosition = value;
      }
    }
  }

  private int _yPosition = 0;
  public int yPosition
  {
    get
    {
      return _yPosition;
    }
    set
    {
      if (value < 0)
      {
        _yPosition = 0;
      }
      else if (value >= map.gridSizeY)
      {
        _yPosition = map.gridSizeY - 1;
      }
      else
      {
        _yPosition = value;
      }
    }
  }

  private int health = 0;
  public int Health
  {
    get { return health; }
    set
    {
      health = value;
      if (health < 0)
      {
        Death();
      }
      if (health > MaxHealth)
      {
        health = MaxHealth;
      }
    }
  }
  public int MaxHealth { get; }

  public string Faction { get; set; }
  public char Symbol { get; set; }

  public int ProductionSpeed { get; } = 5;

  private string resourceType;
  public int resourcesGenerated { get; private set; } = 0;
  public int resourcesGeneratedPerRound { get; set; }
  public int _ResourcePoolRemaining { get; set; } = 0;
  public int ResourcePoolRemaining
  {
    get { return _ResourcePoolRemaining; }
    set
    {
      _ResourcePoolRemaining += value;
      if (_ResourcePoolRemaining >= 100)
      {
        _ResourcePoolRemaining = 100;
      }
    }
  }

  public void GenerateResources()
  {
    resourcesGenerated += resourcesGeneratedPerRound;
    ResourcePoolRemaining += resourcesGeneratedPerRound;
  }

  public void Death()
  {
    map.PlaceUnit(map.blankTile, xPosition, yPosition);
    Destroy(this.gameObject);
  }
}
