using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WizardUnit : MonoBehaviour
{

  public Map map;
  public int xPosition;
  public int yPosition;

  public int maxHealth { get; set; }
  public int health;

  public int speed;

  public int attack;
  public int attackRange;

  public string faction { get; set; }
  public char symbol;

  public bool isAttacking;

  public bool isDead = false;
  public string Name;

  public void Move(Map.Direction direction)
  {
    switch (direction)
    {
      case Map.Direction.Up:
        map.UpdateUnitPosition(this.gameObject, this.yPosition + 1, this.xPosition);
        this.yPosition += 1;
        break;
      case Map.Direction.Down:
        map.UpdateUnitPosition(this.gameObject, this.yPosition - 1, this.xPosition);
        this.yPosition -= 1;
        break;
      case Map.Direction.Left:
        map.UpdateUnitPosition(this.gameObject, this.yPosition, this.xPosition + 1);
        this.xPosition -= 1;
        break;
      case Map.Direction.Right:
        map.UpdateUnitPosition(this.gameObject, this.yPosition, this.xPosition + 1);
        this.xPosition -= 1;
        break;
      default:
        break;
    }
  }

  public void EngageUnit(Unit aTarget)
  {
    isAttacking = true;
    aTarget.health -= this.attack;
  }

  public bool RangeCheck(Unit aTarget)
  {
    int differenceInXPosition = Math.Abs(this.xPosition - aTarget.xPosition);
    int differenceInYPosition = Math.Abs(this.yPosition - aTarget.yPosition);

    if (differenceInXPosition <= attackRange && differenceInYPosition <= attackRange)
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  public GameObject FindClosestEnemy(GameObject[,] aFieldToCheck)
  {
    Unit unitFound = null;

    int rangeToCheck = 1;
    int minRange;
    int maxRange;

    while (unitFound == null)
    {
      minRange = this.xPosition - rangeToCheck;
      maxRange = this.xPosition + rangeToCheck;

      if (minRange < 0)
      {
        minRange = 0;
      }
      if (maxRange > map.gridSizeX)
      {
        maxRange = map.gridSizeX;
      }

      //Check row
      for (int i = minRange; i < maxRange; i++)
      {
        if (aFieldToCheck[i, minRange] != null)
        {
          return aFieldToCheck[i, minRange];
        }
      }
      for (int i = minRange; i < maxRange; i++)
      {
        if (aFieldToCheck[i, maxRange - 1] != null)
        {
          return aFieldToCheck[i, maxRange - 1];
        }
      }

      minRange = this.yPosition - rangeToCheck;
      maxRange = yPosition + rangeToCheck;

      if (minRange < 0)
      {
        minRange = 0;
      }
      if (maxRange > map.gridSizeY)
      {
        maxRange = map.gridSizeY;
      }

      //Check column
      for (int i = minRange; i < maxRange; i++)
      {
        if (aFieldToCheck[i, maxRange - 1] != null)
        {
          return aFieldToCheck[i, maxRange - 1];
        }
      }
      for (int i = minRange; i < maxRange; i++)
      {
        if (aFieldToCheck[i, minRange] != null)
        {
          return aFieldToCheck[i, minRange];
        }
      }
      rangeToCheck++;
    }

    return null;
  }


  public void KillUnit()
  {
    map.PlaceUnit(map.blankTile, xPosition, yPosition);
    Destroy(this.gameObject);
  }
}
