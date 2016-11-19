//Controls our ships
var myType : ShipType; //0/Fighter | 1/Harvester
var selected = false;
var bullet : GameObject; //gameobject
var myRigidbody : Rigidbody2D;
var fireSound : AudioClip;
var explosionSound : AudioClip;
var speed : float = 0.1; //How quickly do we accelerate?
var turnSpeed : float = 1.5; //How fast can we rotate and change orientation?
var cooldown : float = 0.5; //How long until we can fire our weapon?
var maxHealth : float = 0.1; //How much health can we have if fully healed?
var maxSpeed : float = 250; //How fast can we move?
var fireForce : float = 100; //How fast does our bullet move?
var explosionParticle : GameObject; //Explosion effect when we die :(
private var fireTimer : float = 0; //Cooldown timer. You shouldn't have to screw with this.
private var shipHealth : float = maxHealth; //current Health. This should stay private, to help stop weird health modification bugs
private var harvesterAttached : boolean = false;
var target : GameObject;
function Start(){
	myRigidbody = this.GetComponent(Rigidbody2D);
	shipHealth = maxHealth;
}
function AIFollowPlayer(){ //Move towards player position. Lazily attempt to keep a 20m distance.
	var target = GameObject.FindGameObjectWithTag("SelectedShip");
	//this.transform.LookAt(target.transform);
	this.transform.up = target.transform.position - transform.position; //Lazy 2D look at
	this.transform.rotation.eulerAngles.x = 0;
	myRigidbody.AddForce(this.transform.up * speed);
	if(Vector3.Distance(this.transform.position,target.transform.position) <= 20){
		myRigidbody.AddForce(-this.transform.up * speed); //The most lazy fix ever. This is how Half Life 2 "Limited" velocity. Except y'know, 3D.
	}
}
function AIScanForEnemies(){
	if(myType ==0){ //Fighter
		var targets = GameObject.FindGameObjectsWithTag("SpikeEnemy"); //Prioritize spike enemies
		var checktarget = GameObject.FindGameObjectWithTag("SpikeEnemy");
		if(checktarget == null){
			targets = GameObject.FindGameObjectsWithTag("Enemy");
		}
		if(targets.length > 0){
			var lowestDistance = 50; //Scan within  radius
			var ourTarget = 0; //Which ship array index?
			for(var i = 0; i < targets.length; i++){ //Scan through enemies
				if(Vector3.Distance(targets[i].transform.position,this.transform.position) <= lowestDistance){
					ourTarget = i;
					lowestDistance = Vector3.Distance(targets[i].transform.position,this.transform.position);
				}
			}
			target = targets[ourTarget]; //Target found. Let's move on to the next thing!
		}
		else{
			target = null;
		}
	}
}
function AIFireAtTarget(){
	if(target != null){
		this.transform.up = target.transform.position - transform.position;
		this.transform.rotation.eulerAngles.x = 0;
		Fire(); //Wait, is it really that easy?
	}
	else{
		//Debug.Log("No target!");
	}
}

function maintainSpeed(){ //Slow down when above speed parameters
	if(selected == true){
		if(myRigidbody.velocity.magnitude >= maxSpeed){
			myRigidbody.drag = 20;
		}
		else{
			myRigidbody.drag = 0.1;
		}
	}
	else{
		if(myRigidbody.velocity.magnitude >= maxSpeed*0.5){ //Bottleneck speed when we aren't controlling it!
			myRigidbody.drag = 20;
		}
		else{
			myRigidbody.drag = 0.1;
		}
	}
}
function AIScanForMinerals(){ //Clone scanForEnemies, just with a different tag.
	var targets = GameObject.FindGameObjectsWithTag("MineralAsteroid"); //Find minerals only
	if(targets.length > 0){ //This should basically be the same as == null
		var lowestDistance = 50; //Scan within  radius
		var ourTarget = 0; //Which ship array index?
		for(var i = 0; i < targets.length; i++){ //Scan through enemies
			if(Vector3.Distance(targets[i].transform.position,this.transform.position) <= lowestDistance){
				ourTarget = i;
				lowestDistance = Vector3.Distance(targets[i].transform.position,this.transform.position);
			}
		}
		target = targets[ourTarget]; //Target found. Let's move on to the next thing!
	}
	else{
		target = null;
	}
}
function AIMoveToTarget(){ //Move to target, with lazy spacing.
	this.transform.up = target.transform.position - transform.position; //Lazy 2D look at
	this.transform.rotation.eulerAngles.x = 0;
	myRigidbody.AddForce(this.transform.up * speed);
	if(Vector3.Distance(this.transform.position,target.transform.position) <= 5){
		myRigidbody.AddForce(-this.transform.up * speed); //The most lazy fix ever. This is how Half Life 2 "Limited" velocity. Except y'know, 3D.
	}
}
function myAIUpdate(){ //Check AI Mode, Ship Type, and act accordingly.
	var myMode = GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").myAI;
	if(myType == 1){ //Harvester
		if(harvesterAttached == false){
			AIScanForMinerals();
			AIMoveToTarget();
			maintainSpeed();
			Fire();
		}
	}
	else{
		if(myMode == 2){ //Drift
			//Do Nothing
		}
		else{
			if(myMode == 1){ //Follow
				AIFollowPlayer();
				maintainSpeed();
				AIScanForEnemies();
				AIFireAtTarget();
			}
			else{
				if(myMode == 0){ //Sentry
					if(myType == 0){ //Fighter
						myRigidbody.velocity*=0.9;
						AIScanForEnemies();
						AIFireAtTarget();
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
					*/ //Deprecated. We should now make Harvesters ALWAYS harvest.
				}
			}
		}
	}
}
function Fire(){ //Shoot a bullet, or catch an Asteroid in a Harvester. No Caching! :(
	if(fireTimer <= 0.01){
		fireTimer = cooldown;
		if(myType == 0){ //Fighter
			var myBullet = Instantiate(bullet,this.transform.position, Quaternion.identity);
			Physics2D.IgnoreCollision(myBullet.GetComponent(BoxCollider2D),this.GetComponent(BoxCollider2D));
			myBullet.GetComponent(Rigidbody2D).velocity = myRigidbody.velocity;
			myBullet.GetComponent(Rigidbody2D).AddForce(this.transform.up*fireForce);
			this.GetComponent(AudioSource).PlayOneShot(fireSound, 0.5);
		}
		if(myType == 1){ //Harvester 
			Debug.Log("HarvesterFire!");
			//Lock on to the nearest asteroid?
			var myCast : RaycastHit2D = Physics2D.Raycast(this.transform.position,this.transform.up,15);
			if(myCast.collider != null){
				Debug.Log("Harvester Hit!");
				if(myCast.transform.tag == "MineralAsteroid"){
					Debug.Log("Changed parent!");
					this.transform.parent = myCast.collider.gameObject.transform; //Stick to the asteroid
					selected = false; //Remove control!
					myRigidbody.isKinematic = true;
					harvesterAttached = true;
				}
				if(myCast.transform.tag == "Player"){
					Debug.Log("We hit ourselves!!!"); //Because this was a problem a while back.
				}
			}
		}
	}
}
function ifAttachedGetMinerals(){
	if(harvesterAttached == true){
		if(this.transform.parent.gameObject.tag == "MineralAsteroid" ||this.transform.parent.gameObject.tag == "Asteroid"){ //We can harvest minerals
			if(this.transform.parent.gameObject.GetComponent("Asteroid").minerals > 0){
				if(Time.deltaTime > 0){ //Limit it by time, not framerate
					GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals+=1;
					this.transform.parent.gameObject.GetComponent("Asteroid").minerals-=1;
				}
			}
			else{
				this.transform.parent= GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform;
				harvesterAttached = false;
				myRigidbody.isKinematic = false; //Just in case.
			}
		}
	}
}
function shipInput(){ //Check for button presses, act accordingly.
	//Movement
	this.transform.localRotation.eulerAngles.z -= Input.GetAxisRaw("Horizontal") * turnSpeed *Time.deltaTime*14;
	myRigidbody.AddForce(this.transform.up*Input.GetAxisRaw("Vertical")*speed); //2D physics
	if(Input.GetButton("Slow")){
		myRigidbody.velocity*=0.9;
	}
	if(Input.GetButton("Fire")){
		Fire();
	}
}
function weaponTimer(){ //Iterate through the shooting cooldown.
	if(fireTimer > 0){
		fireTimer-=Time.deltaTime;
	}
	if(fireTimer < 0){
		fireTimer=0;
	}
}
function Die(){ //Spawn an explosion, and delete the GameObject.
	var explosion = Instantiate(explosionParticle,this.transform.position,Quaternion.identity);
	var mySound = new GameObject(); //Spawn explosion audio GameObject. Could've sworn there was a better way for this.
	mySound.transform.position = this.transform.position;
	mySound.AddComponent(AudioSource);
	mySound.GetComponent(AudioSource).spatialBlend = 1;
	mySound.GetComponent(AudioSource).PlayOneShot(explosionSound, 1);
	Destroy(mySound, 10); //Cleanup!
	Destroy(explosion, 20);
	Destroy(this.gameObject); //This should be changed, probably.
}
function OnCollisionEnter2D(mycol : Collision2D){ //Check if we're being hurt/shot at
	if(mycol.collider.tag == "SpikeEnemy"){
		shipHealth-=1;
	}
	if(mycol.collider.tag == "EnemyBullet"){
		shipHealth-=0.5;
		Destroy(mycol.gameObject);
	}
}
function Update () { //Called every frame
	if(shipHealth <= 0){
		Die();
	}
	if(selected == true){ //Are we controlling this ship?
		myRigidbody.isKinematic = false;
		shipInput();
		this.transform.tag = "SelectedShip";
	}
	else{ //Let the AI do the things!
		this.transform.tag = "Ship";
		if(myType == 1){ //Harvester
			ifAttachedGetMinerals(); //Harvest!
		}
		myAIUpdate();
	}
	weaponTimer();
}