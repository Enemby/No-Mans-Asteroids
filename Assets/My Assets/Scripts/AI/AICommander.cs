using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AICommander : MonoBehaviour
{
    public CommanderType myType; //0-defense 1-offense 2-balanced
    public float myMinerals;
    public GameObject[] myFighters;
    public GameObject[] myCurrentShips;
    public float thinkTimer; //How often should the AI send orders?
    public float agroTime; //How long have we been waiting for something to happen?
    public bool attacked; //Are we provoked!
    private float internalClock; //Checking time differences.
    private int maxShips;
    public GameObject debugTarget;
    private GameObject[] myStations;
    private GameObject[] playerStations;
    private GameObject[] neutralStations;
    public virtual void typeMaxShips()//Set max ships according to commander type
    {
        //TODO: Change with Commander Types.
        this.maxShips = 7;
    }

    public virtual void Start()
    {
        this.typeMaxShips();
    }

    public virtual void updateReferences() //Update the vars used in other functions
    {
        this.myStations = GameObject.FindGameObjectsWithTag("EnemyStation");
        this.neutralStations = GameObject.FindGameObjectsWithTag("NeutralStation");
        this.myCurrentShips = (GameObject[]) Boo.Lang.Runtime.RuntimeServices.AddArrays(typeof(GameObject), GameObject.FindGameObjectsWithTag("Enemy"), GameObject.FindGameObjectsWithTag("SpikeEnemy"));
        this.updatePlayerStations();
    }

    public virtual void updatePlayerStations() //This solution is so stupid it I'm giving it a seperate function to celebrate.
    {
        int maxCount = GameObject.Find("Base").transform.childCount;
        if (maxCount > 0)
        {
            Component[] myPStations = GameObject.Find("Base").GetComponentsInChildren(typeof(Transform));
            this.playerStations = new GameObject[GameObject.Find("Base").transform.childCount];
            int actualStations = 0;
            i = 0;
            while (i < (myPStations.Length - 1))
            {
                if ((myPStations[i].gameObject.name == "Player Station(Clone)") || (myPStations[i].gameObject.name == "Player Station"))
                {
                    this.playerStations[actualStations] = myPStations[i].gameObject;
                    actualStations = actualStations + 1;
                }
                i++;
            }
        }
    }

    public virtual void DefendStation()
    {
        if (this.myStations.Length == 1)
        {
            i = 0;
            while (i < this.myCurrentShips.Length)
            {
                myObj = new GameObject();
                myObj.transform.position = this.myStations[0].transform.position + (Random.insideUnitCircle * 200);
                this.setShipTarget(this.myCurrentShips[i], myObj);
                UnityEngine.Object.Destroy(myObj, 25);
                i++;
            }
        }
    }

    public virtual GameObject closestTarget(GameObject[] targets, int lowestDistance)
    {
        //var lowestDistance = 50; //Scan within  radius
        object ourTarget = null; //Which ship array index?
        int i = 0;
        while (i < targets.Length) //Scan through enemies
        {
            if (Vector2.Distance(targets[i].transform.position, this.transform.position) <= lowestDistance)
            {
                ourTarget = i;
                lowestDistance = (int) Vector2.Distance(targets[i].transform.position, this.transform.position);
            }
            i++;
        }
        if (!(ourTarget == null))
        {
            return targets[ourTarget]; //Target found. Let's move on to the next thing!
        }
    }

    public virtual void collectResources() //simplified, for optimization
    {
        if (GameObject.FindGameObjectsWithTag("EnemyStation").Length > 0)
        {
            float addedMinerals = Time.deltaTime + GameObject.FindGameObjectsWithTag("EnemyStation").Length;
            addedMinerals = addedMinerals * 0.5f; //Adjust mining speed as needed
            this.myMinerals = this.myMinerals + addedMinerals;
        }
    }

    public virtual void setShipTarget(GameObject theShip, GameObject theTarget)
    {
        if ((theShip.transform.tag == "Enemy") || (theShip.transform.tag == "SpikeEnemy"))//Cool, not a garbage var.
        {
            theShip.GetComponent("enemyShip").target = theTarget;
        }
    }

    public virtual void DesperationCheck()//Assess Resources, make a plan.
    {
        if (this.myStations.Length < 1) //Oh shit!
        {
            //Debug.Log("No EStations!");
            if (this.neutralStations.Length > 0)//Well, that's a start.
            {
                //Debug.Log("Found NStations!");
                if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)//We have options!
                {
                    //Debug.Log("Found Ships!");
                    GameObject[] everyShip = this.myCurrentShips;
                    GameObject targetStation = this.closestTarget(this.neutralStations, 5000);
                    if (targetStation)
                    {
                        i = 0;
                        while (i < everyShip.Length)
                        {
                            this.setShipTarget(everyShip[i], targetStation);
                            i++;
                        }
                    }
                }
            }
        }
        this.updateReferences();
        if (this.myStations.Length <= 0)
        {
            if (this.neutralStations.Length <= 0)
            {
                if (this.myCurrentShips.Length <= 0)
                {
                    UnityEngine.Object.Destroy(this.gameObject, 5);
                }
                else
                {
                    i = 0;
                    while (i < this.myCurrentShips.Length)
                    {
                        if (this.playerStations.Length <= 0)
                        {
                            this.setShipTarget(this.myCurrentShips[i], GameObject.FindGameObjectWithTag("SelectedShip"));
                        }
                        i++;
                    }
                }
            }
        }
    }

    public virtual void buyShip(int myIndex)
    {
        //Calculate price
        int cost = myIndex * 100;
        if (this.myMinerals >= cost)
        {
            if (GameObject.FindGameObjectWithTag("EnemyStation") != null) //avoid errors on loss
            {
                UnityEngine.Object.Instantiate(this.myFighters[myIndex], GameObject.FindGameObjectWithTag("EnemyStation").transform.position + new Vector3(Random.Range(0, 20), Random.Range(0, 20), 0), Quaternion.identity);
                this.myMinerals = this.myMinerals - cost;
            }
        }
    }

    public virtual void supplyShips()//TODO: Algorithm to cost-analyze buying ships.
    {
        if (this.myCurrentShips.Length < this.maxShips) //Make sure we don't exceed max.
        {
            if ((this.myMinerals >= (this.myFighters.Length * 100)) && ((this.myCurrentShips.Length - 1) < this.maxShips))
            {
                this.buyShip(this.myFighters.Length - 1);
            }
            else
            {
            }
        }
    }

    public virtual void agroCheck()
    {
        this.agroTime = this.agroTime + Time.deltaTime;
        if (this.myType == (CommanderType) 0) //Defense
        {
            if (this.agroTime >= 200)
            {
                this.attacked = true;
                this.agroTime = 0;
            }
        }
        else
        {
            if (this.myType == (CommanderType) 1) //Offensive
            {
                if (this.agroTime >= 50)
                {
                    this.attacked = true;
                    this.agroTime = 0;
                }
            }
            else
            {
                if (this.myType == (CommanderType) 2) //Balanced
                {
                    if (this.agroTime >= 100)
                    {
                        this.attacked = true;
                        this.agroTime = 0;
                    }
                }
            }
        }
        if (this.attacked == true)
        {
            if (this.myCurrentShips.Length < (this.maxShips * 0.5f))
            {
                this.attacked = false; //Call off the attack.
                this.agroTime = 0;
            }
        }
    }

    public virtual void attackPlayerStation()
    {
        if (this.playerStations.Length >= 1)
        {
            i = 0;
            while (i < ((this.myCurrentShips.Length - this.myStations.Length) - 1))//Leave one ship out of the attack.
            {
                this.setShipTarget(this.myCurrentShips[i].gameObject, this.playerStations[0].gameObject);
                i++;
            }
            if (this.myStations.Length > 0)
            {
                this.setShipTarget(this.myCurrentShips[this.myCurrentShips.Length - 1].gameObject, this.myStations[0].gameObject);
            }
            else
            {
                this.setShipTarget(this.myCurrentShips[this.myCurrentShips.Length - 1].gameObject, this.playerStations[0].gameObject);
            }
        }
    }

    public virtual void captureNeutralStation()
    {
        i = 0;
        while (i < ((this.myCurrentShips.Length - this.myStations.Length) - 1))//Leave one ship out of the attack.
        {
            this.setShipTarget(this.myCurrentShips[i].gameObject, this.neutralStations[0].gameObject);
            i++;
        }
        this.setShipTarget(this.myCurrentShips[this.myCurrentShips.Length - 1].gameObject, this.myStations[0].gameObject);
    }

    public virtual void balancedAttack()//setShipTarget(myCurrentShips[myCurrentShips.Length-1].gameObject,myStations[0].gameObject);
    {
        if ((this.playerStations.Length >= 1) && (this.neutralStations.Length >= 1))
        {
            i = 0;
            while (i < (this.myCurrentShips.Length * 0.5f)) //attack PlayerStation
            {
                this.setShipTarget(this.myCurrentShips[i].gameObject, this.playerStations[0].gameObject);
                i++;
            }
            i = (int) (this.myCurrentShips.Length * 0.5f);
            while (i < (this.myCurrentShips.Length - 2)) //attack PlayerStation
            {
                this.setShipTarget(this.myCurrentShips[i].gameObject, this.neutralStations[0].gameObject);
                i++;
            }
        }
    }

    public virtual void attackedCommanderAction()
    {
        if (this.myType == (CommanderType) 0) //Defense
        {
            if (this.neutralStations.Length <= 0)
            {
                this.captureNeutralStation();
            }
            else
            {
                this.attackPlayerStation();
            }
        }
        else
        {
            if (this.myType == (CommanderType) 1) //Offensive
            {
                if (this.playerStations.Length >= 1)
                {
                    this.attackPlayerStation();
                }
                else
                {
                    this.captureNeutralStation();
                }
            }
            else
            {
                if (this.myType == (CommanderType) 2) //Balanced
                {
                    this.balancedAttack();
                }
            }
        }
    }

    public virtual void Update()
    {
        this.agroCheck();
        this.collectResources();
        this.updateReferences();
        if (this.internalClock < this.thinkTimer)
        {
            this.internalClock = this.internalClock + Time.deltaTime;
        }
        else
        {
            this.internalClock = 0;
        }
        if (this.internalClock <= 0.03f)
        {
            if (this.myType == (CommanderType) 0) //Defensive
            {
                this.DesperationCheck();
                this.DefendStation();
            }
            else
            {
                if (this.myType == (CommanderType) 1)//Offensive
                {
                    this.DesperationCheck();
                }
                else
                {
                    if (this.myType == (CommanderType) 2)//Balanced 
                    {
                        this.DesperationCheck();
                        this.DefendStation();
                    }
                }
            }
            this.supplyShips();
        }
        if (this.attacked == true)
        {
            this.attackedCommanderAction(); //Response is different for different commanders.
        }
    }

    public AICommander()
    {
        this.myMinerals = 100;
        this.thinkTimer = 0.5f;
        this.maxShips = 10;
    }

}