 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game_Engine : MonoBehaviour
{
  public Map map;
  public static int roundsCompleted = 1;
  public int numberOfUnits;

  public float tickerTimeStart = 3f;
  public float tickerTime = 3f;

  private void Start()
  {
    numberOfUnits = (int)(map.gridSizeX * map.gridSizeY / 2);
    map.numberOfUnits = numberOfUnits;
    StartGame();
  }

  private void Update()
  {
    if (tickerTime <= 0)
    {
      StartNewRound();

      tickerTime = tickerTimeStart;
    }
    else
    {
      tickerTime -= Time.deltaTime;
    }
  }

  public void StartGame()
  {
    map.GenerateBattlefield(numberOfUnits);
    map.UpdateDisplay();
  }

  public void StartNewRound()
  {
    //Perform unit actions
    for (int i = 0; i < map.units.Count; i++)
    {
      if (map.units[i].GetComponent<Unit>() != null)
      {
        PerformAction(map.units[i]);
      }
    }

    //Perform building actions
    for (int i = 0; i < map.buildings.Count; i++)
    {
      if (map.buildings[i].GetComponent<FactoryBuilding>() != null)
      {
        FactoryBuilding building = map.buildings[i].GetComponent<FactoryBuilding>();
        building.resourceBuilding.GenerateResources();
        CheckBuildingProduction(building);
      }
    }

    roundsCompleted++;
  }

  private void CheckBuildingProduction(FactoryBuilding aBuilding)
  {
    if (aBuilding.resourceBuilding.ResourcePoolRemaining >= 5)
    {
      aBuilding.BuildNewUnit(numberOfUnits);
      numberOfUnits++;
    }
  }

  private void PerformAction(GameObject aUnit)
  {
    if (aUnit.GetComponent<Unit>() != null)
    {
      Unit lUnit = aUnit.GetComponent<Unit>();
      GameObject tClosestEnemy = lUnit.FindClosestEnemy(map.Battlefield);

      if (tClosestEnemy != null && lUnit.health >= 0.25 * lUnit.maxHealth)
      {
        Unit closestEnemy = tClosestEnemy.GetComponent<Unit>();
        if (lUnit.RangeCheck(closestEnemy))
        {
          lUnit.EngageUnit(closestEnemy);
          lUnit.isAttacking = true;
        }
        else if (roundsCompleted % lUnit.speed == 0)
        {
          //Move toward the enemy
          int differenceInXPosition = Math.Abs(lUnit.xPosition - closestEnemy.xPosition);
          int differenceInYPosition = Math.Abs(lUnit.yPosition - closestEnemy.yPosition);
          if (differenceInXPosition > differenceInYPosition)
          { //Move vertical
            if (lUnit.yPosition <= closestEnemy.yPosition)
            {
              lUnit.Move(Map.Direction.Up);
            }
            else if (lUnit.yPosition > closestEnemy.yPosition)
            {
              lUnit.Move(Map.Direction.Down);
            }
          }
          else if (differenceInXPosition > differenceInYPosition)
          { //Move horizontal
            if (lUnit.xPosition <= closestEnemy.xPosition)
            {
              lUnit.Move(Map.Direction.Right);
            }
            else if (lUnit.xPosition > closestEnemy.xPosition)
            {
              lUnit.Move(Map.Direction.Left);
            }
          }
          else
          {
            lUnit.Move(Map.Direction.Up);
          }

        }
        else if (lUnit.health < 0.25 * lUnit.maxHealth)
        {
          lUnit.Move(RandomDirection());
        }
      }
    }

    if (aUnit.GetComponent<WizardUnit>() != null)
    {
      WizardUnit lUnit = aUnit.GetComponent<WizardUnit>();
      GameObject tClosestEnemy = lUnit.FindClosestEnemy(map.Battlefield);

      if (tClosestEnemy != null && lUnit.health >= 0.25 * lUnit.maxHealth)
      {
        Unit closestEnemy = tClosestEnemy.GetComponent<Unit>();
        if (lUnit.RangeCheck(closestEnemy))
        {
          lUnit.EngageUnit(closestEnemy);
          lUnit.isAttacking = true;
        }
        else if (roundsCompleted % lUnit.speed == 0)
        {
          //Move toward the enemy
          int differenceInXPosition = Math.Abs(lUnit.xPosition - closestEnemy.xPosition);
          int differenceInYPosition = Math.Abs(lUnit.yPosition - closestEnemy.yPosition);
          if (differenceInXPosition > differenceInYPosition)
          { //Move vertical
            if (lUnit.yPosition <= closestEnemy.yPosition)
            {
              lUnit.Move(Map.Direction.Up);
            }
            else if (lUnit.yPosition > closestEnemy.yPosition)
            {
              lUnit.Move(Map.Direction.Down);
            }
          }
          else if (differenceInXPosition > differenceInYPosition)
          { //Move horizontal
            if (lUnit.xPosition <= closestEnemy.xPosition)
            {
              lUnit.Move(Map.Direction.Right);
            }
            else if (lUnit.xPosition > closestEnemy.xPosition)
            {
              lUnit.Move(Map.Direction.Left);
            }
          }
          else
          {
            lUnit.Move(Map.Direction.Up);
          }

        }
        else if (lUnit.health < 0.25 * lUnit.maxHealth)
        {
          lUnit.Move(RandomDirection());
        }
      }
    }
  }

  private Map.Direction RandomDirection()
  {
    int r = UnityEngine.Random.Range(0, 4);
    switch (r)
    {
      case 0:
        return Map.Direction.Up;
      case 1:
        return Map.Direction.Down;
      case 2:
        return Map.Direction.Left;
      case 3:
        return Map.Direction.Right;
      default:
        return Map.Direction.Up;
    }
  }
}