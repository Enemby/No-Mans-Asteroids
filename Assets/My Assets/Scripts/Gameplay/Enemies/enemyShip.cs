using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class enemyShip : MonoBehaviour
{
    //Customizable Enemy Ships
    [UnityEngine.Header("Ship Type")]
    public ShipType myType; // 0/Fighter 1/Harvester 2/Kamikazae 3/Carrier --Defined in Game.js
    public GameObject target; //Chase Selected Ship!
    public float speed; //Our speed of movement
    public float health; //Our health
    public int maxSpeed; //Our MAX speed
    [UnityEngine.Header("BulletVars")]
    public GameObject bullet;
    public GameObject explosionParticle;
    public AudioClip explosionSound;
    public float cooldown;
    public float fireTimer;
    public float fireForce;
    [UnityEngine.Header("Carrier Vars")]
    public GameObject[] shipSpawns;
    public float[] distanceChecks; //How close does the player have to be?
    private Rigidbody2D myRigidbody;
    private bool beenHit;
    private GameObject aiCom;
    public virtual void Start()
    {
        this.myRigidbody = (Rigidbody2D) this.GetComponent(typeof(Rigidbody2D));
        if (!GameObject.FindGameObjectWithTag("EAICOM"))
        {
            this.aiCom = null;
        }
        else
        {
            this.aiCom = GameObject.FindGameObjectWithTag("EAICOM"); //We don't technically need this..
        }
    }

    public virtual bool commanderCheck()//Are we receiving target orders?
    {
        if (this.aiCom == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public virtual bool orderCompleteCheck()
    {
        if (Vector3.Distance(this.transform.position, this.target.transform.position) <= 5)
        {
            return true;
        }
        return false;
    }

    public virtual void weaponTimer() //Iterate through the shooting cooldown.
    {
        if (this.fireTimer > 0)
        {
            this.fireTimer = this.fireTimer - Time.deltaTime;
        }
        if (this.fireTimer < 0)
        {
            this.fireTimer = 0;
        }
    }

    public virtual float getPlayerDistance() //Distance from our ship
    {
        GameObject player = GameObject.FindGameObjectWithTag("SelectedShip");
        return Vector3.Distance(this.transform.position, player.transform.position);
    }

    public virtual object spawnEscalation(float distance) //Check distance, and spawn accordingly
    {
        int i = 0;
        while (i < (this.shipSpawns.Length - 1)) //Iterate through array
        {
            if (distance <= this.distanceChecks[i]) //Is distance less than our requirement?
            {
                return i; //Return our valid index...
            }
            else
            {
                //This doesn't work right. But it works well enough..
                return null;
            }
            i++;
        }
    }

    public virtual void scanForTargets() //Look for enemies within a reasonable range.
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Ship");
        if (targets.Length > 0)
        {
            int lowestDistance = 150;
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
            if (GameObject.FindGameObjectWithTag("SelectedShip") != null)
            {
                if (Vector2.Distance(this.transform.position, GameObject.FindGameObjectWithTag("SelectedShip").transform.position) <= lowestDistance)
                {
                    this.target = GameObject.FindGameObjectWithTag("SelectedShip").gameObject;
                }
            }
            if (!(ourTarget == null))
            {
                this.target = targets[ourTarget]; //Target found. Let's move on to the next thing!
            }
        }
        else
        {
            if (targets.Length == 0)
            {
                if (GameObject.FindGameObjectWithTag("SelectedShip") != null)
                {
                    if (Vector2.Distance(this.transform.position, GameObject.FindGameObjectWithTag("SelectedShip").transform.position) <= 150)
                    {
                        this.target = GameObject.FindGameObjectWithTag("SelectedShip").gameObject;
                    }
                }
            }
        }
    }

    public virtual void LateUpdate()
    {
        /*
		if(GameObject.FindGameObjectsWithTag("Enemy").Length >= 75 && myType != 3){
			Die(); //Optimization
		}
		*/
        //This is dumb
        if (this.myType == (ShipType) 0) //Fighter..?
        {
            this.scanForTargets();
            this.moveToFireRange();
            if (this.target != null)
            {
                if ((this.target.gameObject.tag == "Ship") || (this.target.gameObject.tag == "SelectedShip"))
                {
                    if (Vector3.Distance(this.transform.position, this.target.transform.position) <= 90)
                    {
                        this.Fire();
                    }
                }
            }
        }
        if (this.myType == (ShipType) 2) //Kamikaze
        {
            if (!this.commanderCheck()) //If there's no commands, find our own targets.
            {
                this.target = GameObject.FindGameObjectWithTag("SelectedShip");
            }
            else
            {
                if (this.target == null)
                {
                    this.scanForTargets();
                }
                else
                {
                    //If we have a target..
                    if (GameObject.FindGameObjectWithTag("SelectedShip") != null)
                    {
                        if (Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("SelectedShip").transform.position) < 100)
                        {
                            this.target = GameObject.FindGameObjectWithTag("SelectedShip");
                        }
                        GameObject[] playerships = GameObject.FindGameObjectsWithTag("Ship");
                        int i = 0;
                        while (i < playerships.Length)
                        {
                            if (Vector3.Distance(this.transform.position, playerships[i].transform.position) < 100)
                            {
                                this.target = playerships[i];
                            }
                            i++;
                        }
                    }
                }
            }
            if (this.target != null) //Stop from throwing errors at start.
            {
                //var targetTransform = target.transform.position - transform.position;
                //this.transform.up = Vector3.MoveTowards(this.transform.up,targetTransform,0.001 * Time.deltaTime);
                this.transform.up = this.target.transform.position - this.transform.position; //Lazy 2D look at
                this.transform.rotation.eulerAngles.x = 0;
                ((Rigidbody2D) this.GetComponent(typeof(Rigidbody2D))).AddForce(this.transform.up * this.speed);
            }
        }
        if (this.myType == (ShipType) 3) //Carrier
        {
            if (this.fireTimer <= 0.01f)
            {
                if (!(this.spawnEscalation(this.getPlayerDistance()) == null))
                {
                    if ((GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("SpikeEnemy").Length) <= 65) //Don't spawn mroe than we can handle
                    {
                        GameObject myShip = UnityEngine.Object.Instantiate(this.shipSpawns[this.spawnEscalation(this.getPlayerDistance())], this.transform.position, Quaternion.identity); //This is kind of hacky...
                        this.Fire(); //Reset fire timer
                    }
                }
            }
        }
        if (this.myType == (ShipType) 7) //NonCombantant. Won't attack until hit.
        {
            if (this.beenHit == true)
            {
                this.scanForTargets();
                this.moveToFireRange();
                if (this.target != null)
                {
                    if ((this.target.gameObject.tag == "Ship") || (this.target.gameObject.tag == "SelectedShip"))
                    {
                        if (Vector3.Distance(this.transform.position, this.target.transform.position) <= 90)
                        {
                            this.Fire();
                        }
                    }
                }
            }
            else
            {
                this.moveToFireRange();
                if (this.fireTimer == 0)
                {
                    this.randomPatrol();
                    this.fireTimer = 25;
                }
            }
        }
        if (this.health <= 0)
        {
            this.Die();
        }
        this.maintainSpeed();
        this.weaponTimer();
    }

    public virtual void moveToFireRange()
    {
        if (this.target != null)
        {
            this.transform.up = this.target.transform.position - this.transform.position; //Lazy 2D look at
            this.transform.rotation.eulerAngles.x = 0;
            this.myRigidbody.AddForce(this.transform.up * this.speed);
            if (Vector3.Distance(this.transform.position, this.target.transform.position) <= 5)
            {
                this.myRigidbody.AddForce(-this.transform.up * this.speed); //The most lazy fix ever. This is how Half Life 2 "Limited" velocity. Except y'know, 2D.
                this.maintainSpeed();
            }
        }
    }

    public virtual void Fire() //Shoot a bullet, No Caching! :(
    {
        if (this.fireTimer <= 0.01f)
        {
            this.fireTimer = this.cooldown;
            if (this.myType != (ShipType) 2) //Check we're not kamikaze, which can't fire.
            {
                GameObject myBullet = UnityEngine.Object.Instantiate(this.bullet, this.transform.position, Quaternion.identity);
                Physics2D.IgnoreCollision((BoxCollider2D) myBullet.GetComponent(typeof(BoxCollider2D)), (BoxCollider2D) this.GetComponent(typeof(BoxCollider2D)));
                if (this.myRigidbody.velocity.magnitude > 1)
                {
                    ((Rigidbody2D) myBullet.GetComponent(typeof(Rigidbody2D))).AddForce(((this.transform.up * this.fireForce) * this.myRigidbody.velocity.magnitude) * 0.1f);
                }
                else
                {
                    //myBullet.GetComponent(Rigidbody2D).velocity = myRigidbody.velocity;
                    ((Rigidbody2D) myBullet.GetComponent(typeof(Rigidbody2D))).velocity = ((Rigidbody2D) myBullet.GetComponent(typeof(Rigidbody2D))).velocity * 1.5f;
                }
            }
            if (this.myType == (ShipType) 3) //Carrier!
            {
            }
        }
    }

    public virtual void randomPatrol() //Pick a random location to move toward
    {
        myObj = new GameObject();
        myObj.transform.position = Vector3.zero + (Random.insideUnitCircle * 200);
        this.target = myObj;
        UnityEngine.Object.Destroy(myObj, 25);
    }

    public virtual void Die()
    {
        GameObject explosion = UnityEngine.Object.Instantiate(this.explosionParticle, this.transform.position, Quaternion.identity);
        GameObject mySound = new GameObject(); //Spawn explosion audio GameObject. Could've sworn there was a better way for this.
        mySound.transform.position = this.transform.position;
        mySound.AddComponent(typeof(AudioSource));
        ((AudioSource) mySound.GetComponent(typeof(AudioSource))).spatialBlend = 0.7f;
        ((AudioSource) mySound.GetComponent(typeof(AudioSource))).PlayOneShot(this.explosionSound, 1);
        GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("ScreenShake");
        UnityEngine.Object.Destroy(mySound, 10); //Cleanup!
        UnityEngine.Object.Destroy(this.gameObject); //This should be changed, probably.
    }

    public virtual void maintainSpeed() //If we're going too fast, slow down!
    {
        if (((Rigidbody2D) this.GetComponent(typeof(Rigidbody2D))).velocity.magnitude >= this.maxSpeed)
        {
            ((Rigidbody2D) this.GetComponent(typeof(Rigidbody2D))).drag = 20;
        }
        else
        {
            ((Rigidbody2D) this.GetComponent(typeof(Rigidbody2D))).drag = 0.1f;
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D mycol)
    {
        if (mycol.collider.tag == "Bullet")
        {
            this.health = this.health - 0.5f;
            UnityEngine.Object.Destroy(mycol.collider.gameObject); //Sanity check
            if (this.myType == (ShipType) 7) //NonCombantant
            {
                this.beenHit = true;
                this.fireTimer = 0;
            }
        }
    }

    public enemyShip()
    {
        this.myType = (ShipType) 2;
        this.speed = 200;
        this.health = 3;
        this.maxSpeed = 400;
        this.cooldown = 1;
        this.fireForce = 500;
    }

}