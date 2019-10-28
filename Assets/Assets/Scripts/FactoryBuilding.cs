using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBuilding : MonoBehaviour
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

  public GameObject ResourceBuilding;
  public ResourceBuilding resourceBuilding
  {
    get
    {
      return ResourceBuilding.GetComponent<ResourceBuilding>();
    }
    set
    {
      ResourceBuilding RB = ResourceBuilding.GetComponent<ResourceBuilding>() as ResourceBuilding;
      RB  = value;
    }
  }

  private int health = 0;
  public int Health
  { get { return health;  }
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

  public Map.UnitTypes UnitType;

  public int ProductionSpeed { get; } = 5;

  public int SpawnPoint
  {
    get
    {
      if (this.yPosition > 0)
      {
        return this.yPosition - 1;
      }
      else
      {
        return this.yPosition + 1;
      }

    }
  }

  public void BuildNewUnit(int aUnitNumber)
  {
    if (resourceBuilding != null)
    {
      GameObject lUnit;
      if (UnitType == Map.UnitTypes.Green)
      {
        lUnit = map.GenerateRandomGreenUnit(aUnitNumber, this.xPosition, SpawnPoint);
      }
      else //(UnitType == Map.UnitTypes.Red)
      {
        lUnit = map.GenerateRandomRedUnit(aUnitNumber, this.xPosition, SpawnPoint);
      }

      resourceBuilding.ResourcePoolRemaining -= 5;
    }
  }

  public void Death()
  {
    map.PlaceUnit(map.blankTile, xPosition, yPosition);
    Destroy(this.gameObject);
  }

}
