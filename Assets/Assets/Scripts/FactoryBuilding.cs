using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBuilding : MonoBehaviour
{
  public Map map;
  public int xPosition
  {
    get { return xPosition; }
    set
    {
      if (value < 0)
      {
        xPosition = 0;
      }
      else if (value >= map.gridSizeX)
      {
        xPosition = map.gridSizeX - 1;
      }
      else
      {
        xPosition = value;
      }
    }
  }
  public int yPosition
  {
    get
    {
      return yPosition;
    }
    set
    {
      if (value < 0)
      {
        yPosition = 0;
      }
      else if (value >= map.gridSizeY)
      {
        yPosition = map.gridSizeY - 1;
      }
      else
      {
        yPosition = value;
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


  public int Health
  { get { return Health;  }
    set
    {
      Health = value;
      if (Health < 0)
      {
        Death();
      }
      if (Health > MaxHealth)
      {
        Health = MaxHealth;
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
    Destroy(this);
  }

}
