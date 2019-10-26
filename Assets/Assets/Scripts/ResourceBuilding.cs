using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuilding : MonoBehaviour
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

  public int Health
  {
    get { return Health; }
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
    Destroy(this);
  }
}
