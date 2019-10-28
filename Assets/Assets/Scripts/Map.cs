using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
  public List<GameObject> redUnits;
  public List<GameObject> greenUnits;
  public List<GameObject> grayUnits;
  public GameObject blankTile;
  public GameObject factoryBuildingRed;
  public GameObject factoryBuildingGreen;
  public GameObject resourceBuilding;

  public enum Direction { Up, Down, Left, Right };
  public int gridSizeX = 20;
  public int gridSizeY = 20;
  public GameObject[,] Battlefield;

  public int numberOfUnits = 20;
  public List<GameObject> units;
  public List<GameObject> buildings;

  public enum UnitTypes { Red, Green}


  
  public void DrawBlankMap()
  {
    Battlefield = new GameObject[gridSizeX, gridSizeY];
    for (int i = 0; i < gridSizeX; i++)
    {
      for (int j = 0; j < gridSizeY; j++)
      {
        Battlefield[i, j] = blankTile;
        Instantiate(blankTile, GetDisplayCoord(i, j), Quaternion.identity);
      }
    }
  }

  public void UpdateDisplay()
  {
    DrawBlankMap();

    foreach (GameObject unit in units)
    {
      if (unit.GetComponent<Unit>() != null)
      {
        Unit lUnit = unit.GetComponent<Unit>();
        PlaceUnit(unit, lUnit.xPosition, lUnit.yPosition);
      }
      else if (unit.GetComponent<WizardUnit>() != null)
      {
        WizardUnit lUnit = unit.GetComponent<WizardUnit>();
        PlaceUnit(unit, lUnit.xPosition, lUnit.yPosition);
      }
    }

    foreach (GameObject building in buildings)
    {
      if (building.GetComponent<FactoryBuilding>() != null)
      {//place Factories
        FactoryBuilding lBuilding = building.GetComponent<FactoryBuilding>();
        PlaceUnit(building, lBuilding.xPosition, lBuilding.yPosition);
      }
      if (building.GetComponent<ResourceBuilding>() != null)
      {//Place Resource Buildings
        ResourceBuilding lBuilding = building.GetComponent<ResourceBuilding>();
        PlaceUnit(building, lBuilding.xPosition, lBuilding.yPosition);
      }
    }
  }

  public void PlaceUnit(GameObject aUnit, int aXPos, int aYPos)
  {
    if (Battlefield[aXPos, aYPos] == null || Battlefield[aXPos, aYPos] == blankTile)
    {
      Battlefield[aXPos, aYPos] = aUnit;
      Instantiate(aUnit, GetDisplayCoord(aXPos, aYPos), Quaternion.identity);
    }
    else
    {
      Destroy(Battlefield[aXPos, aYPos]);
      Battlefield[aXPos, aYPos] = aUnit;
      Instantiate(aUnit, GetDisplayCoord(aXPos, aYPos), Quaternion.identity);
    }
  }

  private bool CheckPositionAgainstGridSize(int x, int y)
  {
    if (x >= gridSizeX)
    {
      x = gridSizeX - 1;
      return false;
    }
  
    if (y >= gridSizeY)
    {
      y = gridSizeY - 1;
      return false;
    }

    if (x < 0)
    {
      x = 0;
      return false;
    }

    if (y < 0)
    {
      y = 0;
      return false;
    }
    return true;
  }

  public void UpdateUnitPosition(GameObject aUnitToMove, int aXNewPos, int aYNewPos)
  {
    int newX = aXNewPos;
    int newY = aYNewPos;
    if (!CheckPositionAgainstGridSize(newY, newY))
    {
      if (newX >= gridSizeX)
      {
        newX = gridSizeX - 1;
      }
      if (newY >= gridSizeY)
      {
        newY = gridSizeY - 1;
      }
      if (newX < 0)
      {
        newX = 0;
      }
      if (newY < 0)
      {
        newY = 0;
      }
    }

    if (aUnitToMove.GetComponent<Unit>() != null)
    {
      if (Battlefield[newX, newY] == blankTile)
      {
        Unit lUnitToMove = aUnitToMove.GetComponent<Unit>();

        CheckPositionAgainstGridSize(lUnitToMove.xPosition, lUnitToMove.yPosition);

        int currentPosX = lUnitToMove.xPosition;
        int currentPosY = lUnitToMove.yPosition;

        GameObject unit = Battlefield[currentPosX, currentPosY];

        Battlefield[currentPosX, currentPosY] = Battlefield[newX, newY];
        Battlefield[newX, newY] = unit;

        Battlefield[currentPosX, currentPosY].transform.position = GetDisplayCoord(currentPosX, currentPosY);
        Battlefield[newX, newY].transform.position = GetDisplayCoord(newX, newY);
      }
    }
    else if (aUnitToMove.GetComponent<WizardUnit>() != null)
    {
      WizardUnit lUnitToMove = aUnitToMove.GetComponent<WizardUnit>();

      CheckPositionAgainstGridSize(lUnitToMove.xPosition, lUnitToMove.yPosition);

      int currentPosX = lUnitToMove.xPosition;
      int currentPosY = lUnitToMove.yPosition;

      GameObject unit = Battlefield[currentPosX, currentPosY];

      Battlefield[currentPosX, currentPosY] = Battlefield[aXNewPos, aYNewPos];
      Battlefield[aXNewPos, aYNewPos] = unit;

      Battlefield[currentPosX, currentPosY].transform.position = GetDisplayCoord(currentPosX, currentPosY);
      Battlefield[aXNewPos, aYNewPos].transform.position = GetDisplayCoord(aXNewPos, aYNewPos);
    }

  }

  private Vector2 GetDisplayCoord(int aXpos, int aYPos)
  {
    float xPos = (float)(0.16 + (aXpos * 0.32));
    float yPos = (float)(0.16 + (aYPos * 0.32));
    Vector2 position = new Vector2(xPos, yPos);
    return position;
  }

  public void GenerateBattlefield(int aNumberOfUnits)
  {
    Battlefield = new GameObject[gridSizeX, gridSizeY];
    for (int i = 0; i < aNumberOfUnits / 2; i++)
    {
      Vector2 position = GetRandomOpenPosition();
      GameObject unit = GenerateRandomUnit(i);
      PlaceUnit(unit, (int)position.x, (int)position.y);
      units.Add(unit);
    }

    for (int i = 0; i < aNumberOfUnits / 8; i++)
    {
      Vector2 position = GetRandomOpenPosition();
      GameObject building = GenerateFactory(i);
      PlaceUnit(building, (int)position.x, (int)position.y);
      buildings.Add(building);
    }

    for (int i = 0; i < aNumberOfUnits / 4; i++)
    {
      Vector2 position = GetRandomOpenPosition();
      GameObject wizard = GenerateWizard(i, (int)position.x, (int)position.y);
      PlaceUnit(wizard, (int)position.x, (int)position.y);
      units.Add(wizard);
    }

    for (int i = 0; i < gridSizeX; i++)
    {
      for (int j = 0; j < gridSizeY; j++)
      {
        if (Battlefield[i, j] == null)
        {
          PlaceUnit(blankTile, i, j);
        }
      }
    }
  }

  public Vector2 GetRandomOpenPosition()
  {
    Vector2 position = new Vector2();

    bool posFound = false;
    do
    {
      int x = Random.Range(0, gridSizeX);
      int y = Random.Range(0, gridSizeY);
      if ( Battlefield[x, y] == null || Battlefield[x, y] == blankTile)
      {
        posFound = true;
        position.x = x;
        position.y = y;
      }
    }
    while (!posFound);

    return position;
  }

  public GameObject GenerateFactory(int aUnitNumber)
  {
    GameObject lFactory = new GameObject();
    FactoryBuilding building;
    Vector2 point = GetRandomOpenPosition();

    int xPos = (int)point.x;
    int yPos = (int)point.y;

    int team = Random.Range(0, 2);
    if (team == 0)
    {
      lFactory = Instantiate(factoryBuildingRed, GetDisplayCoord((int)point.x, (int)point.y), Quaternion.identity);
      building = lFactory.GetComponent<FactoryBuilding>();
      building.UnitType = UnitTypes.Red;
      building.Faction = "Red";
      building.Symbol = 'R';
    }
    else
    {
      lFactory = Instantiate(factoryBuildingGreen, GetDisplayCoord((int)point.x, (int)point.y), Quaternion.identity);
      building = lFactory.GetComponent<FactoryBuilding>();
      building.UnitType = UnitTypes.Green;
      building.Faction = "Green";
      building.Symbol = 'G';
    }

    building.map = this;
    building.name = $"aUnitNumber";
    building.xPosition = xPos;
    building.yPosition = yPos;
    building.Health = 12;
    PlaceUnit(lFactory, building.xPosition, building.yPosition);
    building.ResourceBuilding = GenerateResourceBuilding(aUnitNumber++);

    buildings.Add(lFactory);

    if (lFactory == null)
    {
      return factoryBuildingRed;
    }
    else
    {
    return lFactory;
    }
  }
  public GameObject GenerateResourceBuilding(int aUnitNumber)
  {
    Vector2 point = GetRandomOpenPosition();
    GameObject lBuilding = Instantiate(resourceBuilding, GetDisplayCoord((int)point.x, (int)point.y), Quaternion.identity);

    int xPos = (int)point.x;
    int yPos = (int)point.y;

    ResourceBuilding building = lBuilding.GetComponent<ResourceBuilding>();

    building.map = this;
    building.name = $"aUnitNumber";
    building.xPosition = xPos;
    building.yPosition = yPos;
    building.Health = 12;
    building.resourcesGeneratedPerRound = 1;

    PlaceUnit(lBuilding, xPos, yPos);
    //buildings.Add(lBuilding);
    return lBuilding;
  }

  private GameObject GenerateRandomUnit(int aUnitNumber)
  {
    GameObject lUnit = blankTile;
    Vector2 point = GetRandomOpenPosition();

    int xPos = (int)point.x;
    int yPos = (int)point.y;

    int team = Random.Range(0, 2);
    if (team == 0)
    {
      lUnit = GenerateRandomRedUnit(aUnitNumber, xPos, yPos);
    }
    else if (team == 1)
    {
      lUnit = GenerateRandomGreenUnit(aUnitNumber, xPos, yPos);
    }

    return lUnit;
  }

  public GameObject GenerateWizard(int aUnitNumber, int aXPos, int aYPos)
  {
    int unitType = Random.Range(0, redUnits.Capacity);
    GameObject lUnit = Instantiate(grayUnits[unitType]);
    WizardUnit unit = lUnit.GetComponent<WizardUnit>();

    unit.Name = $"{aUnitNumber}";
    unit.attackRange = 1;
    unit.map = this;
    unit.xPosition = aXPos;
    unit.yPosition = aYPos;
    unit.health = 12;
    unit.speed = Random.Range(1, 4);
    unit.attack = Random.Range(1, 6);
    unit.faction = "Red";
    unit.symbol = 'R';

    return lUnit;
  }

  public GameObject GenerateRandomRedUnit(int aUnitNumber, int aXPos, int aYPos)
  {
    int unitType = Random.Range(0, redUnits.Capacity);
    GameObject lUnit = Instantiate(redUnits[unitType]);
    Unit unit = lUnit.GetComponent<Unit>();

    if (unitType == 0)
    {
      unit.attackRange = Random.Range(2, 6);
    }
    else
    {
      unit.attackRange = 1;
    }
    //unit.name = $"{aUnitNumber}";
    unit.map = this;
    unit.xPosition = aXPos;
    unit.yPosition = aYPos;
    unit.health = 12;
    unit.speed = Random.Range(1, 4);
    unit.attack = Random.Range(1, 6);
    unit.faction = "Red";
    unit.symbol = 'R';

    return lUnit;
  }
  public GameObject GenerateRandomGreenUnit(int aUnitNumber, int aXPos, int aYPos)
  {
    int unitType = Random.Range(0, greenUnits.Capacity);
    GameObject lUnit = Instantiate(greenUnits[unitType]);
    Unit unit = lUnit.GetComponent<Unit>();

    if (unitType == 0)
    {
      unit.attackRange = Random.Range(2, 6);
    }
    else
    {
      unit.attackRange = 1;
    }
    //unit.name = $"{aUnitNumber}";
    unit.map = this;
    unit.xPosition = aXPos;
    unit.yPosition = aYPos;
    unit.health = 12;
    unit.speed = Random.Range(1, 4);
    unit.attack = Random.Range(1, 6);
    unit.faction = "Green";
    unit.symbol = 'G';

    return lUnit;
  }
  public GameObject GenerateRandomGrayUnit(int aUnitNumber, int aXPos, int aYPos)
  {
    int unitType = Random.Range(0, grayUnits.Capacity);
    GameObject lUnit = Instantiate(grayUnits[unitType]);
    Unit unit = lUnit.GetComponent<Unit>();

    unit.map = this;
    unit.attackRange = 1;
    unit.Name = $"{aUnitNumber}";
    unit.xPosition = aXPos;
    unit.yPosition = aYPos;
    unit.health = 12;
    unit.speed = Random.Range(1, 4);
    unit.attack = Random.Range(1, 6);
    unit.faction = "Gray";
    unit.symbol = 'W';

    return lUnit;
  }
}
