using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PlayerShip : MonoBehaviour
{
    //Controls our ships
    public ShipType myType; //0/Fighter | 1/Harvester
    public bool selected;
    public bool allowInput;
    public GameObject bullet; //gameobject
    public Rigidbody2D myRigidbody;
    public AudioClip fireSound;
    public AudioClip explosionSound;
    public AudioClip damagedSound;
    public AudioClip slowSound;
    public float speed; //How quickly do we accelerate?
    public float turnSpeed; //How fast can we rotate and change orientation?
    public float cooldown; //How long until we can fire our weapon?
    public float maxHealth; //How much health can we have if fully healed?
    public float maxSpeed; //How fast can we move?
    public float maxRange;
    public float fireForce; //How fast does our bullet move?
    public GameObject explosionParticle; //Explosion effect when we die :(
    [UnityEngine.Header("Station Settings")]
    public GameObject[] ships;
    public int[] prices;
    public GameObject shopCanvas;
    public AudioClip buySound;
    public float captureProgress;
    public float captureTime;
    public GameObject playerStation;
    public GameObject neutralStation;
    public GameObject enemyStation;
    public float interactTimer; //Decrease capture after certain time
    private float fireTimer; //Cooldown timer. You shouldn't have to screw with this unless something goes terribly wrong!
    public float shipHealth; //current Health. This should stay private, to help stop weird health modification bugs
    private bool harvesterAttached;
    public GameObject basicHarvester;
    public GameObject target;
    public virtual void Start()
    {
        this.myRigidbody = (Rigidbody2D) this.GetComponent(typeof(Rigidbody2D));
        this.shipHealth = this.maxHealth;
        this.setCorrectSquad();
        this.setStationStart();
    }

    public virtual void AIFollowPlayer() //Move towards player position. Lazily attempt to keep a 20m distance.
    {
        if (GameObject.FindGameObjectWithTag("SelectedShip") != null)
        {
            GameObject target = GameObject.FindGameObjectWithTag("SelectedShip");
            Component targetRB = target.GetComponent("Rigidbody2D");
            //this.transform.LookAt(target.transform);
            this.transform.up = target.transform.position - this.transform.position; //Lazy 2D look at
            this.transform.rotation.eulerAngles.x = 0;
            this.myRigidbody.AddForce(this.transform.up * this.speed);
            if (Vector3.Distance(this.transform.position, target.transform.position) <= 20)
            {
                if (this.myRigidbody.velocity.magnitude > targetRB.velocity.magnitude)
                {
                    this.myRigidbody.velocity = this.myRigidbody.velocity * 0.9f; //Slow down to target's speed when in range.
                }
            }
        }
    }

    public virtual void setStationStart()
    {
        if (this.myType == (ShipType) 5) //Station Check
        {
            this.shipHealth = 99999;
            if (this.shopCanvas == null)
            {
                this.shopCanvas = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Shop Panel").gameObject;
            }
            Debug.Log(GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).gameObject.name);
            if (GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).transform.childCount == 0)
            {
                UnityEngine.Object.Instantiate(this.basicHarvester, this.transform.position, Quaternion.identity);
            }
        }
    }

    public virtual void setCorrectSquad() //Or set initial parent, to be literal.
    {
        if (this.transform.parent == null)
        {
            if (this.myType == (ShipType) 0) //Fighter
            {
                this.transform.parent = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(0).transform;
            }
            else
            {
                //This could be improved..
                if (this.myType == (ShipType) 1) //Harvester
                {
                    this.transform.parent = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).transform;
                }
                else
                {
                    if (this.myType == (ShipType) 5) //Station
                    {
                        this.transform.parent = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(2).transform;
                        this.setStationStart();
                    }
                    else
                    {
                        if (this.myType == (ShipType) 6) //Turret
                        {
                            this.transform.parent = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(3).transform;
                        }
                    }
                }
            }
        }
    }

    public virtual void AIScanForEnemies()
    {
        if (this.myType == (ShipType) 0) //Fighter
        {
            bool spikeCheck = false;
            GameObject[] targets = GameObject.FindGameObjectsWithTag("SpikeEnemy");
            if (targets.Length == 0) //Prioritize spike enemies
            {
                targets = GameObject.FindGameObjectsWithTag("Enemy");
                spikeCheck = true;
            }
            if (targets.Length > 0)
            {
                int lowestDistance = 90;
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
                    this.target = targets[ourTarget]; //Target found. Let's move on to the next thing!
                }
                else
                {
                    this.target = null;
                    spikeCheck = true;
                }
            }
            else
            {
                this.target = null;
            }
            if (spikeCheck == true)
            {
                targets = GameObject.FindGameObjectsWithTag("Enemy");
                if (targets.Length > 0)
                {
                    int lowestDistance2 = 90;
                    object ourTarget2 = null; //Which ship array index?
                    int i2 = 0;
                    while (i2 < targets.Length) //Scan through enemies
                    {
                        if (Vector2.Distance(targets[i2].transform.position, this.transform.position) <= lowestDistance2)
                        {
                            ourTarget2 = i2;
                            lowestDistance2 = (int) Vector2.Distance(targets[i2].transform.position, this.transform.position);
                        }
                        i2++;
                    }
                    if (!(ourTarget2 == null))
                    {
                        this.target = targets[ourTarget2]; //Target found. Let's move on to the next thing!
                    }
                    else
                    {
                        this.target = null;
                        spikeCheck = true;
                    }
                }
                else
                {
                    this.target = null;
                }
            }
        }
    }

    public virtual void AIFireAtTarget()//Debug.Log("No target!");
    {
        if (this.target != null)
        {
            this.transform.up = this.target.transform.position - this.transform.position;
            this.transform.rotation.eulerAngles.x = 0;
            this.Fire(); //Wait, is it really that easy?
        }
        else
        {
        }
    }

    public virtual void maintainSpeed() //Slow down when above speed parameters
    {
        if (this.selected == true)
        {
            if (this.myRigidbody.velocity.magnitude >= this.maxSpeed)
            {
                this.myRigidbody.drag = 20;
            }
            else
            {
                this.myRigidbody.drag = 0.1f;
            }
        }
        else
        {
            if (this.myRigidbody.velocity.magnitude >= (this.maxSpeed * 0.5f)) //Bottleneck speed when we aren't controlling it!
            {
                this.myRigidbody.drag = 20;
            }
            else
            {
                this.myRigidbody.drag = 0.1f;
            }
        }
    }

    public virtual void AIScanForMinerals() //Clone scanForEnemies, just with a different tag.
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("MineralAsteroid");
        if (targets.Length > 0) //This should basically be the same as == null
        {
            int lowestDistance = 50;
            int ourTarget = 0; //Scan within  radius
            int i = 0; //Which ship array index?
            while (i < targets.Length) //Scan through enemies
            {
                if (Vector3.Distance(targets[i].transform.position, this.transform.position) <= lowestDistance)
                {
                    ourTarget = i;
                    lowestDistance = (int) Vector3.Distance(targets[i].transform.position, this.transform.position);
                }
                i++;
            }
            if (this.target == null)
            {
                this.target = targets[ourTarget]; //Target found. Let's move on to the next thing!
            }
            else
            {
                if (this.target.tag != "MineralAsteroid")
                {
                    this.target = null;
                }
            }
        }
        else
        {
            this.target = null;
        }
    }

    public virtual void AIMoveToTarget() //Move to target, with lazy spacing.
    {
        if (this.target != null)
        {
            this.transform.up = this.target.transform.position - this.transform.position; //Lazy 2D look at
            this.transform.rotation.eulerAngles.x = 0;
            this.myRigidbody.AddForce(this.transform.up * this.speed);
            if (Vector3.Distance(this.transform.position, this.target.transform.position) <= 5)
            {
                this.myRigidbody.AddForce(-this.transform.up * this.speed); //The most lazy fix ever. This is how Half Life 2 "Limited" velocity. Except y'know, 3D.
            }
        }
    }

    /*
					else{
						if(myType == 1){ //Harvester
							if(harvesterAttached == false){
								AIScanForMinerals();
								AIMoveToTarget();
								maintainSpeed();
								Fire();
							}
						}
					}
					*/    public virtual void myAIUpdate() //Check AI Mode, Ship Type, and act accordingly.
    {
        object myMode = GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").myAI;
        if (this.myType == (ShipType) 1) //Harvester
        {
            if (this.harvesterAttached == false)
            {
                this.AIScanForMinerals();
                this.AIMoveToTarget();
                this.maintainSpeed();
                this.Fire();
            }
        }
        else
        {
            if (myMode == 2) //Drift
            {
            }
            else
            {
                //Do Nothing
                if (myMode == 1) //Follow
                {
                    this.AIFollowPlayer();
                    this.maintainSpeed();
                    this.AIScanForEnemies();
                    this.AIFireAtTarget();
                }
                else
                {
                    if (myMode == 0) //Sentry
                    {
                        if (this.myType == (ShipType) 0) //Fighter
                        {
                            this.myRigidbody.velocity = this.myRigidbody.velocity * 0.9f;
                            this.AIScanForEnemies();
                            this.AIFireAtTarget();
                        }
                    }
                }
            }
        }
    }

    public virtual void Fire() //Shoot a bullet, or catch an Asteroid in a Harvester. No Caching! :(
    {
        if (this.fireTimer <= 0.01f)
        {
            this.fireTimer = this.cooldown;
            if (this.myType == (ShipType) 0) //Fighter
            {
                GameObject myBullet = UnityEngine.Object.Instantiate(this.bullet, this.transform.position, Quaternion.identity);
                Physics2D.IgnoreCollision((BoxCollider2D) myBullet.GetComponent(typeof(BoxCollider2D)), (BoxCollider2D) this.GetComponent(typeof(BoxCollider2D)));
                /*
			//I actually though this would work. It did not, but IS SO CLOSE TO FIXING THE PROBLEM that I can't bring myself to remove it.
			myBullet.GetComponent(Rigidbody2D).velocity = this.GetComponent(Rigidbody2D).velocity*2; //Bullets inherit ship velocity
			var myDot = Vector3.Dot(Vector3(myBullet.GetComponent(Rigidbody2D).velocity.x,myBullet.GetComponent(Rigidbody2D).velocity.y,0).normalized, transform.up.normalized); //Check ship velocity
			if(myDot < 0){ //Cut off velocity if negative
				myDot = 0;
			}
			myBullet.GetComponent(Rigidbody2D).velocity *=myDot; //if 1, let velocity go, if 0, cancel any velocity inheritance
			//myDot legit took me like 3 hours to do, don't screw with it if you value your time
			*/
                ((Rigidbody2D) myBullet.GetComponent(typeof(Rigidbody2D))).velocity = this.transform.up * this.fireForce;
                ((Rigidbody2D) myBullet.GetComponent(typeof(Rigidbody2D))).velocity = ((Rigidbody2D) myBullet.GetComponent(typeof(Rigidbody2D))).velocity + ((Rigidbody2D) this.GetComponent(typeof(Rigidbody2D))).velocity; //0.5 is more correct, but makes you feel like you shot at an angle at high speeds.
                ((AudioSource) this.GetComponent(typeof(AudioSource))).PlayOneShot(this.fireSound, 0.5f);
            }
            if (this.myType == (ShipType) 1) //Harvester 
            {
                Debug.Log("HarvesterFire!");
                //Lock on to the nearest asteroid?
                RaycastHit2D myCast = Physics2D.Raycast(this.transform.position, this.transform.up, 15);
                if (myCast.collider != null)
                {
                    Debug.Log("Harvester Hit!");
                    if (myCast.transform.tag == "MineralAsteroid")
                    {
                        Debug.Log("Changed parent!");
                        this.transform.parent = myCast.collider.gameObject.transform; //Stick to the asteroid
                        this.selected = false; //Remove control!
                        this.myRigidbody.isKinematic = true;
                        this.harvesterAttached = true;
                    }
                    if (myCast.transform.tag == "Player")
                    {
                        Debug.Log("We hit ourselves!!!"); //Because this was a problem a while back.
                    }
                }
            }
        }
    }

    public virtual void ifAttachedGetMinerals()
    {
        if (this.harvesterAttached == true)
        {
            if (this.transform.parent != null)
            {
                if ((this.transform.parent.gameObject.tag == "MineralAsteroid") || (this.transform.parent.gameObject.tag == "Asteroid")) //We can harvest minerals
                {
                    if (this.transform.parent.gameObject.GetComponent("Asteroid").minerals > 0)
                    {
                        if (Time.deltaTime > 0) //Limit it by time, not framerate
                        {
                            GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals = ((int) GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals) + 1;
                            this.transform.parent.gameObject.GetComponent("Asteroid").minerals = ((int) this.transform.parent.gameObject.GetComponent("Asteroid").minerals) - 1;
                        }
                    }
                    else
                    {
                        this.transform.parent = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).transform;
                        this.harvesterAttached = false;
                        this.myRigidbody.isKinematic = false; //Just in case.
                    }
                }
            }
            else
            {
                this.transform.parent = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).transform;
                this.harvesterAttached = false;
                this.myRigidbody.isKinematic = false;
            }
        }
    }

    public virtual void shipInput() //Check for button presses, act accordingly.
    {
        //Movement
        if (this.allowInput == true)
        {
            this.transform.localRotation.eulerAngles.z = this.transform.localRotation.eulerAngles.z - (((Input.GetAxisRaw("Horizontal") * this.turnSpeed) * Time.deltaTime) * 14);
            this.myRigidbody.AddForce((this.transform.up * Input.GetAxisRaw("Vertical")) * this.speed); //2D physics
            this.myRigidbody.AddForce(((this.transform.right * Input.GetAxisRaw("Strafe")) * this.speed) * 0.5f); //2D physics
            if (Input.GetButton("Slow"))
            {
                this.myRigidbody.velocity = this.myRigidbody.velocity * 0.9f;
                if (this.myRigidbody.velocity.magnitude >= 10)
                {
                    ((AudioSource) this.GetComponent(typeof(AudioSource))).clip = this.slowSound;
                    if (((AudioSource) this.GetComponent(typeof(AudioSource))).isPlaying == false)
                    {
                        ((AudioSource) this.GetComponent(typeof(AudioSource))).Play();
                        ((AudioSource) this.GetComponent(typeof(AudioSource))).loop = true;
                    }
                }
                else
                {
                    ((AudioSource) this.GetComponent(typeof(AudioSource))).Stop();
                }
            }
            else
            {
                ((AudioSource) this.GetComponent(typeof(AudioSource))).Stop();
                ((AudioSource) this.GetComponent(typeof(AudioSource))).loop = false;
                ((AudioSource) this.GetComponent(typeof(AudioSource))).clip = null;
            }
            if (Input.GetButton("Fire"))
            {
                this.Fire();
            }
        }
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

    public virtual void Die() //Spawn an explosion, and delete the GameObject.
    {
        GameObject explosion = UnityEngine.Object.Instantiate(this.explosionParticle, this.transform.position, Quaternion.identity);
        GameObject mySound = new GameObject(); //Spawn explosion audio GameObject. Could've sworn there was a better way for this.
        mySound.transform.position = this.transform.position;
        mySound.AddComponent(typeof(AudioSource));
        ((AudioSource) mySound.GetComponent(typeof(AudioSource))).spatialBlend = 0.5f;
        ((AudioSource) mySound.GetComponent(typeof(AudioSource))).PlayOneShot(this.explosionSound, 1);
        GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("ScreenShake");
        UnityEngine.Object.Destroy(mySound, 10); //Cleanup!
        UnityEngine.Object.Destroy(explosion, 20);
        UnityEngine.Object.Destroy(this.gameObject); //This should be changed, probably.
    }

    public virtual void OnCollisionStay2D(Collision2D otherobj)//Check if any ships are touching me!
    {
        if (this.myType == (ShipType) 5)//If Station
        {
            if ((otherobj.gameObject.tag == "Ship") || (otherobj.gameObject.tag == "SelectedShip"))//Player Capture!
            {
                this.captureProgress = this.captureProgress + Time.deltaTime;
                this.interactTimer = 5;
            }
            else
            {
                if (otherobj.gameObject.tag == "Enemy") //Enemy Capture!
                {
                    this.captureProgress = this.captureProgress - Time.deltaTime;
                    this.interactTimer = 5;
                }
            }
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D mycol) //Check if we're being hurt/shot at
    {
        if (mycol.collider.tag == "SpikeEnemy")
        {
            this.shipHealth = this.shipHealth - 1;
        }
        if (mycol.collider.tag == "EnemyBullet")
        {
            this.shipHealth = this.shipHealth - 0.5f;
            if ((this.selected == true) && this.checkSquadSelect())
            {
                GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("ScreenShake");
                GameObject mySound = new GameObject(); //Spawn explosion audio GameObject. Could've sworn there was a better way for this.
                mySound.transform.position = this.transform.position;
                mySound.AddComponent(typeof(AudioSource));
                ((AudioSource) mySound.GetComponent(typeof(AudioSource))).spatialBlend = 0;
                ((AudioSource) mySound.GetComponent(typeof(AudioSource))).pitch = ((AudioSource) mySound.GetComponent(typeof(AudioSource))).pitch + (Random.Range(0, 1) * 0.1f);
                ((AudioSource) mySound.GetComponent(typeof(AudioSource))).PlayOneShot(this.damagedSound, 1);
                UnityEngine.Object.Destroy(mySound, 10); //Cleanup!
            }
            UnityEngine.Object.Destroy(mycol.gameObject);
        }
    }

    public virtual void shopInput()
    {
        i = 1;
        while (i < this.ships.Length) //This is the laziest fix ever. Let me explain why we start at 1.
        {
            //It's so we don't have to include 0 as an input, which our array starts at.
            //However,our KeyDown won't work properly, since the game view shows no 0 variable.
            //So we just pretend 0 doesn't exist, and extend our array arbitrarily. Lazy.
            if (Input.GetKeyDown(i + ""))
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals >= this.prices[i])
                {
                    GameObject myShip = UnityEngine.Object.Instantiate(this.ships[i], this.transform.position + new Vector3(Random.Range(0, 5), Random.Range(0, 5), 0), Quaternion.identity);
                    GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals = ((int) GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals) - this.prices[i];
                    myShip.transform.parent = null; //Don't set parents, kids!
                    ((AudioSource) this.GetComponent(typeof(AudioSource))).PlayOneShot(this.buySound, 1);
                }
            }
            i++;
        }
    }

    public virtual void checkCanvas()
    {
        if (this.selected == true)
        {
            this.shopCanvas.active = true;
        }
        else
        {
            this.shopCanvas.active = false;
        }
    }

    public virtual void clearSelection()
    {
        this.selected = false;
    }

    //LARGE SECTION OF STATION RELATED CODE
    public virtual void spawnPlayerStation()//Player has captured this station!
    {
        Transform myParent = GameObject.FindGameObjectWithTag("Squad Manager").transform.GetChild(2);
        GameObject myStation = UnityEngine.Object.Instantiate(this.playerStation, this.transform.position, this.transform.rotation);
        myStation.transform.parent = myParent.transform;
    }

    public virtual void spawnEnemyStation()//Enemy has captured this station!
    {
        GameObject myStation = UnityEngine.Object.Instantiate(this.enemyStation, this.transform.position, this.transform.rotation);
        myStation.transform.parent = null;
    }

    public virtual void setProgressUI()//Update capture graphics!
    {
        Transform progressBar = this.gameObject.transform.GetChild(0);
        if (this.captureProgress > 0)
        {
            ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.g = this.captureProgress / 5;
            ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.r = 0;
            ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.a = this.interactTimer / 5;
        }
        else
        {
            if (this.captureProgress < 0)
            {
                ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.r = Mathf.Abs(this.captureProgress) / 5;
                ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.g = 0;
                ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.a = this.interactTimer / 5;
            }
            else
            {
                if (progressBar == 0)
                {
                    ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.r = 1;
                    ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.g = 1;
                    ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.b = 1;
                    ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.a = this.interactTimer / 5;
                }
                else
                {
                    ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.b = 0;
                }
            }
        }
    }

    public virtual void checkInteractTimer() //Check time since last capture
    {
        if (this.interactTimer >= 0)
        {
            this.interactTimer = this.interactTimer - Time.deltaTime;
        }
        else
        {
            this.interactTimer = 0;
            this.captureProgress = 0;
        }
    }

    public virtual void CaptureLogic()
    {
        if (this.captureProgress >= this.captureTime)
        {
            this.spawnPlayerStation();
            UnityEngine.Object.Destroy(this.gameObject);
        }
        else
        {
            if (this.captureProgress <= -this.captureTime)
            {
                this.spawnEnemyStation();
                UnityEngine.Object.Destroy(this.gameObject);
            }
        }
    }

    public virtual bool checkSquadSelect() //We check if the base SQUAD is selected
    {
        SquadManager myParent = (SquadManager) this.transform.parent.gameObject.GetComponent(typeof(SquadManager));
        if (myParent.squadSelected == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void updateShop()
    {
        if (this.shopCanvas == null)
        {
            this.shopCanvas = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Shop Panel").gameObject;
        }
    }

    //END MOST OF THE STATION CODE
    public virtual void FixedUpdate()
    {
        if (this.selected == true)
        {
            if (this.shipHealth <= (this.maxHealth - 0.0001f))
            {
                this.shipHealth = this.shipHealth + 0.0001f;
            }
        }
    }

    public virtual void Update() //Called every frame
    {
        if (this.shipHealth <= 0)
        {
            this.Die();
        }
        if (this.myType == (ShipType) 5) //Station
        {
            this.checkInteractTimer();
            this.CaptureLogic();
            this.setProgressUI();
            if (this.selected == true)
            {
                this.shopInput();
                this.checkCanvas();
            }
            if (this.checkSquadSelect() == false)
            {
                this.updateShop();
                this.shopCanvas.active = false;
            }
        }
        else
        {
            if (this.selected == true) //Are we controlling this ship?
            {
                this.myRigidbody.isKinematic = false;
                this.shipInput();
                this.transform.tag = "SelectedShip";
            }
            else
            {
                 //Let the AI do the things!
                this.transform.tag = "Ship";
                if (this.myType == (ShipType) 1) //Harvester
                {
                    this.ifAttachedGetMinerals(); //Harvest!
                }
                this.myAIUpdate();
            }
            this.weaponTimer();
        }
    }

    public virtual float getMaxHealth()
    {
        return this.maxHealth;
    }

    public virtual float getShipHealth()
    {
        return this.shipHealth;
    }

    public PlayerShip()
    {
        this.allowInput = true;
        this.speed = 0.1f;
        this.turnSpeed = 1.5f;
        this.cooldown = 0.5f;
        this.maxHealth = 0.1f;
        this.maxSpeed = 250;
        this.maxRange = 250;
        this.fireForce = 100;
        this.captureTime = 5;
        this.shipHealth = this.maxHealth;
    }

}