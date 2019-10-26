using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
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
  public string name;

  // Start is called before the first frame update
  void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void Move(Map.Direction direction)
  {
    switch (direction)
    {
      case Map.Direction.Up:
        map.UpdateUnitPosition(this.xPosition, this.yPosition, this.yPosition + 1, this.xPosition);
        this.yPosition += 1;
        break;
      case Map.Direction.Down:
        map.UpdateUnitPosition(this.xPosition, this.yPosition, this.yPosition - 1, this.xPosition);
        this.yPosition -= 1;
        break;
      case Map.Direction.Left:
        map.UpdateUnitPosition(this.xPosition, this.yPosition, this.yPosition, this.xPosition + 1);
        this.xPosition -= 1;
        break;
      case Map.Direction.Right:
        map.UpdateUnitPosition(this.xPosition, this.yPosition, this.yPosition, this.xPosition + 1);
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
    GameObject unitFound = null;

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
      if (maxRange > aFieldToCheck.GetLength(1))  //number of columns
      {
        maxRange = aFieldToCheck.GetLength(1);  //number of columns
      }

      //Check rows
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
      if (maxRange > aFieldToCheck.GetLength(0))  //number of rows
      {
        maxRange = aFieldToCheck.GetLength(0);  //number of rows
      }

      //Check columns
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

  public void DamageUnit(int aAttack)
  {
    this.health -= aAttack;
  }

  public void KillUnit()
  {
    Destroy(this);
  }

  //public string ToString()
}
