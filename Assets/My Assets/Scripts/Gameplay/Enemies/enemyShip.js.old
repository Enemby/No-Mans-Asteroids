//Customizable Enemy Ships
@Header("Ship Type")
var myType : ShipType = 2; // 0/Fighter 1/Harvester 2/Kamikazae 3/Carrier --Defined in Game.js
public var target : GameObject; //Chase Selected Ship!
var speed : float = 200; //Our speed of movement
var health : float = 3; //Our health
var maxSpeed = 400; //Our MAX speed
@Header("BulletVars")
var bullet : GameObject;
var explosionParticle : GameObject;
var explosionSound : AudioClip;
var cooldown : float = 1;
var fireTimer : float = 0;
var fireForce : float = 500;
@Header("Carrier Vars")
var shipSpawns : GameObject[]; //What ships can we spawn?
var distanceChecks : float[]; //How close does the player have to be?
private var myRigidbody : Rigidbody2D;
private var beenHit : boolean = false;
private var aiCom : GameObject = null;
function Start(){
	myRigidbody = this.GetComponent(Rigidbody2D);
	if(!GameObject.FindGameObjectWithTag("EAICOM")){
		aiCom = null;
	}
	else{
		aiCom = GameObject.FindGameObjectWithTag("EAICOM"); //We don't technically need this..
	}
}
function commanderCheck(){//Are we receiving target orders?
	if(aiCom == null){
		return false;
	}
	else{
		return true;
	}
}
function orderCompleteCheck(){
	if(Vector3.Distance(this.transform.position,target.transform.position) <= 5){
		return true;
	}
	return false;
}
function weaponTimer(){ //Iterate through the shooting cooldown.
	if(fireTimer > 0){
		fireTimer-=Time.deltaTime;
	}
	if(fireTimer < 0){
		fireTimer=0;
	}
}
function getPlayerDistance(){ //Distance from our ship
	var player = GameObject.FindGameObjectWithTag("SelectedShip");
	return Vector3.Distance(this.transform.position, player.transform.position);
	//ez
}
function spawnEscalation(distance : float){ //Check distance, and spawn accordingly
	for(var i = 0;i < shipSpawns.length-1; i++){ //Iterate through array
		if(distance <= distanceChecks[i]){ //Is distance less than our requirement?
			return i; //Return our valid index...
			//This doesn't work right. But it works well enough..
		}
		else{
			return null;
		}
	}
}
function scanForTargets(){ //Look for enemies within a reasonable range.
	var targets = GameObject.FindGameObjectsWithTag("Ship");
	if(targets.length > 0){
		var lowestDistance = 150; //Scan within  radius
		var ourTarget; //Which ship array index?
		for(var i = 0; i < targets.length; i++){ //Scan through enemies
			if(Vector2.Distance(targets[i].transform.position,this.transform.position) <= lowestDistance){
				ourTarget = i;
				lowestDistance = Vector2.Distance(targets[i].transform.position,this.transform.position);
			}
		}
		if(GameObject.FindGameObjectWithTag("SelectedShip") != null){
			if(Vector2.Distance(this.transform.position,GameObject.FindGameObjectWithTag("SelectedShip").transform.position )<= lowestDistance){
				target = GameObject.FindGameObjectWithTag("SelectedShip").gameObject;
			}
		}
		if(ourTarget != null){
			target = targets[ourTarget]; //Target found. Let's move on to the next thing!
		}
	}
	else{
		if(targets.length == 0){
			if(GameObject.FindGameObjectWithTag("SelectedShip") != null){
				if(Vector2.Distance(this.transform.position,GameObject.FindGameObjectWithTag("SelectedShip").transform.position )<= 150){
					target = GameObject.FindGameObjectWithTag("SelectedShip").gameObject;
				}
			}
		}
	}
}

function LateUpdate () {
		/*
		if(GameObject.FindGameObjectsWithTag("Enemy").Length >= 75 && myType != 3){
			Die(); //Optimization
		}
		*/ //This is dumb
		if(myType == 0){ //Fighter..?
			scanForTargets();
			moveToFireRange();
			if(target != null){
				if(target.gameObject.tag == "Ship" || target.gameObject.tag == "SelectedShip"){
					if(Vector3.Distance(this.transform.position,target.transform.position) <= 90){
						Fire();
					}
				}
			}
		}
		if(myType == 2){ //Kamikaze
			if(!commanderCheck()){ //If there's no commands, find our own targets.
				target = GameObject.FindGameObjectWithTag("SelectedShip");
			}
			else{
				if(target == null){
					scanForTargets();
				}
				else{//If we have a target..
					if(GameObject.FindGameObjectWithTag("SelectedShip") != null){
						if(Vector3.Distance(this.transform.position,GameObject.FindGameObjectWithTag("SelectedShip").transform.position) < 100){
								target = GameObject.FindGameObjectWithTag("SelectedShip");
						}
						var playerships = GameObject.FindGameObjectsWithTag("Ship");
						for(var i = 0;i < playerships.Length;i++){
							if(Vector3.Distance(this.transform.position,playerships[i].transform.position) < 100){
								target = playerships[i];
							}
						}
					}
				}
			}
			if(target != null){ //Stop from throwing errors at start.
				//var targetTransform = target.transform.position - transform.position;
				//this.transform.up = Vector3.MoveTowards(this.transform.up,targetTransform,0.001 * Time.deltaTime);
				this.transform.up = target.transform.position - transform.position; //Lazy 2D look at
				this.transform.rotation.eulerAngles.x = 0;
				this.GetComponent(Rigidbody2D).AddForce(this.transform.up * speed);
			}
		}
		if(myType == 3){ //Carrier
			if(fireTimer <= 0.01){
				if(spawnEscalation(getPlayerDistance()) != null){
					if(GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("SpikeEnemy").Length <= 65) { //Don't spawn mroe than we can handle
						var myShip = Instantiate(shipSpawns[spawnEscalation(getPlayerDistance())],this.transform.position,Quaternion.identity); //This is kind of hacky...
						Fire(); //Reset fire timer
					}
				}
			}
			
		}
		if(myType == 7){ //NonCombantant. Won't attack until hit.
			if(beenHit == true){
				scanForTargets();
				moveToFireRange();
				if(target != null){
					if(target.gameObject.tag == "Ship" || target.gameObject.tag == "SelectedShip"){
						if(Vector3.Distance(this.transform.position,target.transform.position) <= 90){
							Fire();
						}
					}
				}
			}
			else{
				moveToFireRange();
				if(fireTimer == 0){
					randomPatrol();
					fireTimer = 25;
				}
			}
		}
		if(health <= 0){
			Die();
		}
		maintainSpeed();
		weaponTimer();
}
function moveToFireRange(){
	if(target != null){
		
		this.transform.up = target.transform.position - transform.position; //Lazy 2D look at
		this.transform.rotation.eulerAngles.x = 0;
		myRigidbody.AddForce(this.transform.up * speed);
		if(Vector3.Distance(this.transform.position,target.transform.position) <= 5){
			myRigidbody.AddForce(-this.transform.up * speed); //The most lazy fix ever. This is how Half Life 2 "Limited" velocity. Except y'know, 2D.
			maintainSpeed();
		}
	}
}
function Fire(){ //Shoot a bullet, No Caching! :(
	if(fireTimer <= 0.01){
		fireTimer = cooldown;
		if(myType != 2){ //Check we're not kamikaze, which can't fire.
			var myBullet = Instantiate(bullet,this.transform.position, Quaternion.identity);
			Physics2D.IgnoreCollision(myBullet.GetComponent(BoxCollider2D),this.GetComponent(BoxCollider2D));
			if(myRigidbody.velocity.magnitude > 1){
				myBullet.GetComponent(Rigidbody2D).AddForce(this.transform.up*fireForce*myRigidbody.velocity.magnitude*0.1);
				//myBullet.GetComponent(Rigidbody2D).velocity = myRigidbody.velocity;
			}
			else{
				myBullet.GetComponent(Rigidbody2D).velocity*=1.5;
			}
		}
		if(myType == 3){ //Carrier!
			
		}
	}
}
function randomPatrol(){ //Pick a random location to move toward
	myObj = new GameObject();
	myObj.transform.position = Vector3.zero + Random.insideUnitCircle * 200;
	target = myObj;
	Destroy(myObj,25);
}
function Die(){
	var explosion = Instantiate(explosionParticle,this.transform.position,Quaternion.identity);
	var mySound = new GameObject(); //Spawn explosion audio GameObject. Could've sworn there was a better way for this.
	mySound.transform.position = this.transform.position;
	mySound.AddComponent(AudioSource);
	mySound.GetComponent(AudioSource).spatialBlend = 0.7;
	mySound.GetComponent(AudioSource).PlayOneShot(explosionSound, 1);
	GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("ScreenShake");
	Destroy(mySound, 10); //Cleanup!
	Destroy(this.gameObject); //This should be changed, probably.
}
function maintainSpeed(){ //If we're going too fast, slow down!
	if(this.GetComponent(Rigidbody2D).velocity.magnitude >= maxSpeed){
		this.GetComponent(Rigidbody2D).drag = 20;
	}
	else{
		this.GetComponent(Rigidbody2D).drag = 0.1;
	}
}
function OnCollisionEnter2D(mycol : Collision2D){
	if(mycol.collider.tag == "Bullet"){
		health-=0.5;
		Destroy(mycol.collider.gameObject); //Sanity check
		if(myType == 7){ //NonCombantant
			beenHit = true;
			fireTimer=0;
		}
	}
}