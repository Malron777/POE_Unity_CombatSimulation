using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
  public List<GameObject> redUnits;
  public List<GameObject> greenUnits;
  public List<GameObject> grayUnits;
  public GameObject blankTile;
  public GameObject factoryBuilding;
  public GameObject resourceBuilding;

  public enum Direction { Up, Down, Left, Right };
  public int gridSizeX = 20;
  public int gridSizeY = 20;
  public GameObject[,] Battlefield;

  public int numberOfUnits;
  public List<GameObject> units;
  public List<GameObject> buildings;

  public enum UnitTypes { Red, Green}

  // Start is called before the first frame update
  void Start()
  {
    //Battlefield = new GameObject[gridSizeX, gridSizeY];
    //numberOfUnits = gridSizeX * gridSizeY / 2;
  }

  // Update is called once per frame
  void Update()
  {    

  }
  
  private void DrawBlankMap()
  {
    for (int i = 0; i < gridSizeX; i++)
    {
      for (int j = 0; j < gridSizeY; j++)
      {
        PlaceUnit(blankTile, i, j);
      }
    }
  }

  public void UpdateDisplay()
  {
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
    GameObject tUnit = Battlefield[aXPos, aYPos];
    Battlefield[aXPos, aYPos] = aUnit;
    DestroyImmediate(tUnit);
    Instantiate(aUnit, GetDisplayCoord(aXPos, aYPos), Quaternion.identity);
  }

  public void UpdateUnitPosition(int aXCurrentPos, int aYCurrentPos, int aXNewPos, int aYNewPos)
  {
    GameObject unit = Battlefield[aXCurrentPos, aYCurrentPos];
    Destroy(Battlefield[aXCurrentPos, aYCurrentPos]);
    Battlefield[aXCurrentPos, aYCurrentPos] = blankTile;
    Destroy(Battlefield[aXNewPos, aXNewPos]);
    Battlefield[aXNewPos, aXNewPos] = unit;
    Instantiate(Battlefield[aXNewPos, aXNewPos], GetDisplayCoord(aXNewPos, aYNewPos), Quaternion.identity);
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
    for (int i = 0; i < gridSizeX; i++)
    {
      for (int j = 0; j < gridSizeY; j++)
      {
        Battlefield = new GameObject[gridSizeX, gridSizeY];
        //Destroy(Battlefield[i, j]);
        Battlefield[i, j] = blankTile;
      }
    }

    for (int i = 0; i < aNumberOfUnits; i++)
    {
      Vector2 position = GetRandomOpenPosition();
      GameObject unit = GenerateRandomUnit(i);
      PlaceUnit(unit, (int)position.x, (int)position.y);
      units.Add(unit);
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
      position.x = x;
      position.y = y;
      if (Battlefield[x, y] == blankTile)
      {
        posFound = true;
      }
    }
    while (!posFound);

    return position;
  }

  public GameObject GenerateFactory(int aUnitNumber)
  {
    GameObject lFactory = Instantiate(factoryBuilding);
    Vector2 point = GetRandomOpenPosition();

    int xPos = (int)point.x;
    int yPos = (int)point.y;

    FactoryBuilding building = lFactory.GetComponent<FactoryBuilding>();

    int team = Random.Range(0, 2);
    if (team == 0)
    {
      building.UnitType = UnitTypes.Red;
      building.Faction = "Red";
      building.Symbol = 'R';
    }
    else
    {
      building.UnitType = UnitTypes.Green;
      building.Faction = "Green";
      building.Symbol = 'G';
    }

    building.name = $"aUnitNumber";
    building.xPosition = xPos;
    building.yPosition = yPos;
    building.Health = 12;
    PlaceUnit(lFactory, building.xPosition, building.yPosition);
    building.ResourceBuilding = GenerateResourceBuilding(aUnitNumber++);

    buildings.Add(lFactory);
    return lFactory;
  }
  public GameObject GenerateResourceBuilding(int aUnitNumber)
  {
    GameObject lBuilding = Instantiate(resourceBuilding);
    Vector2 point = GetRandomOpenPosition();

    int xPos = (int)point.x;
    int yPos = (int)point.y;

    ResourceBuilding building = lBuilding.GetComponent<ResourceBuilding>();

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

    int team = Random.Range(0, 3);
    if (team == 0)
    {
      lUnit = GenerateRandomRedUnit(aUnitNumber, xPos, yPos);
    }
    else if (team == 1)
    {
      lUnit = GenerateRandomGreenUnit(aUnitNumber, xPos, yPos);
    }
    else if (team == 2)
    {

    }

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
    unit.xPosition = aXPos;
    unit.yPosition = aYPos;
    unit.health = 12;
    unit.speed = Random.Range(1, 4);
    unit.attack = Random.Range(1, 6);
    unit.faction = "Red";
    unit.symbol = 'R';

    units.Add(lUnit);
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
    unit.xPosition = aXPos;
    unit.yPosition = aYPos;
    unit.health = 12;
    unit.speed = Random.Range(1, 4);
    unit.attack = Random.Range(1, 6);
    unit.faction = "Green";
    unit.symbol = 'G';
    units.Add(lUnit);
    return lUnit;
  }
  public GameObject GenerateRandomGrayUnit(int aUnitNumber, int aXPos, int aYPos)
  {
    int unitType = Random.Range(0, grayUnits.Capacity);
    GameObject lUnit = Instantiate(grayUnits[unitType]);
    Unit unit = lUnit.GetComponent<Unit>();

    unit.attackRange = 1;
    unit.name = $"{aUnitNumber}";
    unit.xPosition = aXPos;
    unit.yPosition = aYPos;
    unit.health = 12;
    unit.speed = Random.Range(1, 4);
    unit.attack = Random.Range(1, 6);
    unit.faction = "Gray";
    unit.symbol = 'W';
    units.Add(lUnit);
    return lUnit;
  }
}
