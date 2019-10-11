using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
  public int xPosition;
  public int yPosition;

  private int maxHealth;
  public int health;

  private int speed;

  private int attack;
  private int attackRange;

  protected string faction;
  private GameObject symbol;

  private bool isAttacking;

  public bool isDead = false;

  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void PerformTurn()
  {
    
  }
  public void Move(Map.Direction direction)
  {

  }
  
  public void EngageUnit(Unit aTarget)
  {

  }

  public bool RangeCheck(Unit aTarget)
  {
    return true;
  }

  public void FindClosestEnemy(GameObject[,] aFieldToCheck)
  {

  }

  public void DamageUnit(int aAttack)
  {

  }

  public void KillUnit()
  {
    Destroy(this);
  }

  //public override string ToString()
}
