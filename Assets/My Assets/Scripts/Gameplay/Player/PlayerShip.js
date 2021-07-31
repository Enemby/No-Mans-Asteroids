//Controls our ships
var myType : ShipType; //0/Fighter | 1/Harvester
var selected = false;
var allowInput : boolean = true;
var bullet : GameObject; //gameobject
var myRigidbody : Rigidbody2D;
var fireSound : AudioClip;
var explosionSound : AudioClip;
var speed : float = 0.1; //How quickly do we accelerate?
var turnSpeed : float = 1.5; //How fast can we rotate and change orientation?
var cooldown : float = 0.5; //How long until we can fire our weapon?
public var maxHealth : float = 0.1; //How much health can we have if fully healed?
var maxSpeed : float = 250; //How fast can we move?
var maxRange : float = 250;
var fireForce : float = 100; //How fast does our bullet move?
var explosionParticle : GameObject; //Explosion effect when we die :(
@Header ("Station Settings")
var ships : GameObject[];
var prices : int[];
var shopCanvas : GameObject;
var buySound : AudioClip;
var captureProgress : float = 0;
var captureTime : float = 5;
var playerStation : GameObject;
var neutralStation : GameObject;
var enemyStation : GameObject;
var interactTimer : float = 0; //Decrease capture after certain time
private var fireTimer : float = 0; //Cooldown timer. You shouldn't have to screw with this unless something goes terribly wrong!
public var shipHealth : float = maxHealth; //current Health. This should stay private, to help stop weird health modification bugs
private var harvesterAttached : boolean = false;
var basicHarvester : GameObject;
var target : GameObject;
function Start(){
	myRigidbody = this.GetComponent(Rigidbody2D);
	shipHealth = maxHealth;
	setCorrectSquad();
	setStationStart();
}
function AIFollowPlayer(){ //Move towards player position. Lazily attempt to keep a 20m distance.
	if(	GameObject.FindGameObjectWithTag("SelectedShip") != null){
		var target = GameObject.FindGameObjectWithTag("SelectedShip");
		//this.transform.LookAt(target.transform);
		this.transform.up = target.transform.position - transform.position; //Lazy 2D look at
		this.transform.rotation.eulerAngles.x = 0;
		myRigidbody.AddForce(this.transform.up * speed);
		if(Vector3.Distance(this.transform.position,target.transform.position) <= 20){
			myRigidbody.AddForce(-this.transform.up * speed); //The most lazy fix ever. This is how Half Life 2 "Limited" velocity. Except y'know, 2D.
		}
	}
}
function setStationStart(){
	if(myType == 5){ //Station Check
		shipHealth = 99999;
		if(shopCanvas == null){
			shopCanvas = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Shop Panel").gameObject;
		}
		Debug.Log(GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).gameObject.name);
		if(GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).transform.childCount == 0){
			Instantiate(basicHarvester,this.transform.position,Quaternion.identity);
			
		}
	}
}
function setCorrectSquad(){ //Or set initial parent, to be literal.
	if(this.transform.parent == null){
		if(myType == 0){ //Fighter
			this.transform.parent= GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(0).transform;
			//This could be improved..
		}
		else{
			if(myType == 1){ //Harvester
				this.transform.parent= GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).transform;
			}
			else{
				if(myType == 5){ //Station
					this.transform.parent= GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(2).transform;
					setStationStart();
				}
				else{
					if(myType == 6){ //Turret
						this.transform.parent= GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(3).transform;
					}
				}
			}
		}
	}
}
function AIScanForEnemies(){
	if(myType ==0){ //Fighter
		var spikeCheck : boolean = false;
		var targets = GameObject.FindGameObjectsWithTag("SpikeEnemy"); //Prioritize spike enemies
		if(targets.length == 0){
			targets = GameObject.FindGameObjectsWithTag("Enemy");
			spikeCheck = true;
		}
		if(targets.length > 0){
			var lowestDistance = 90; //Scan within  radius
			var ourTarget; //Which ship array index?
			for(var i = 0; i < targets.length; i++){ //Scan through enemies
				if(Vector2.Distance(targets[i].transform.position,this.transform.position) <= lowestDistance){
					ourTarget = i;
					lowestDistance = Vector2.Distance(targets[i].transform.position,this.transform.position);
				}
			}
			if(ourTarget != null){
				target = targets[ourTarget]; //Target found. Let's move on to the next thing!
			}
			else{
				target = null;
				spikeCheck = true;
			}
		}
		else{
			target = null;
		}
		if(spikeCheck == true){
			targets = GameObject.FindGameObjectsWithTag("Enemy");
			if(targets.length > 0){
			var lowestDistance2 = 90; //Scan within  radius
			var ourTarget2; //Which ship array index?
			for(var i2 = 0; i2 < targets.length; i2++){ //Scan through enemies
				if(Vector2.Distance(targets[i2].transform.position,this.transform.position) <= lowestDistance2){
					ourTarget2 = i2;
					lowestDistance2 = Vector2.Distance(targets[i2].transform.position,this.transform.position);
				}
			}
			if(ourTarget2 != null){
				target = targets[ourTarget2]; //Target found. Let's move on to the next thing!
			}
			else{
				target = null;
				spikeCheck = true;
			}
		}
		else{
			target = null;
		}
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
		if(target == null){
		
			target = targets[ourTarget]; //Target found. Let's move on to the next thing!
		}
		else{
			if(target.tag != "MineralAsteroid"){
				target = null;
			}
		}
	}
	else{
		target = null;
	}
}
function AIMoveToTarget(){ //Move to target, with lazy spacing.
	if(target != null){
		this.transform.up = target.transform.position - transform.position; //Lazy 2D look at
		this.transform.rotation.eulerAngles.x = 0;
		myRigidbody.AddForce(this.transform.up * speed);
		if(Vector3.Distance(this.transform.position,target.transform.position) <= 5){
			myRigidbody.AddForce(-this.transform.up * speed); //The most lazy fix ever. This is how Half Life 2 "Limited" velocity. Except y'know, 3D.
		}
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
			myBullet.GetComponent(Rigidbody2D).velocity = this.transform.up*fireForce;
			myBullet.GetComponent(Rigidbody2D).velocity += this.GetComponent(Rigidbody2D).velocity; //0.5 is more correct, but makes you feel like you shot at an angle at high speeds.
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
		if(this.transform.parent != null){
			if(this.transform.parent.gameObject.tag == "MineralAsteroid" ||this.transform.parent.gameObject.tag == "Asteroid"){ //We can harvest minerals
				if(this.transform.parent.gameObject.GetComponent("Asteroid").minerals > 0){
					if(Time.deltaTime > 0){ //Limit it by time, not framerate
						GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals+=1;
						this.transform.parent.gameObject.GetComponent("Asteroid").minerals-=1;
					}
				}
				else{
					this.transform.parent= GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).transform;
					harvesterAttached = false;
					myRigidbody.isKinematic = false; //Just in case.
				}
			}
		}
		else{
			this.transform.parent= GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform.GetChild(1).transform;
			harvesterAttached = false;
			myRigidbody.isKinematic = false;
		}
	}
}
function shipInput(){ //Check for button presses, act accordingly.
	//Movement
	if(allowInput == true){
		this.transform.localRotation.eulerAngles.z -= Input.GetAxisRaw("Horizontal") * turnSpeed *Time.deltaTime*14;
		myRigidbody.AddForce(this.transform.up*Input.GetAxisRaw("Vertical")*speed); //2D physics
		if(Input.GetButton("Slow")){
			myRigidbody.velocity*=0.9;
		}
		if(Input.GetButton("Fire")){
			Fire();
		}
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
function OnCollisionStay2D(otherobj : Collision2D){//Check if any ships are touching me!
	if(myType == 5){//If Station
		if(otherobj.gameObject.tag == "Ship"||otherobj.gameObject.tag == "SelectedShip"){//Player Capture!
			captureProgress +=Time.deltaTime;
			interactTimer = 5;
		}
		else{
			if(otherobj.gameObject.tag == "Enemy"){ //Enemy Capture!
				captureProgress-=Time.deltaTime;
				interactTimer = 5;
			}
		}
	}
}
function OnCollisionEnter2D(mycol : Collision2D){ //Check if we're being hurt/shot at
	if(mycol.collider.tag == "SpikeEnemy"){
		shipHealth-=1;
	}
	if(mycol.collider.tag == "EnemyBullet"){
		shipHealth-=0.5;
		if(selected == true && checkSquadSelect()){
			GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("ScreenShake");
		}
		Destroy(mycol.gameObject);
	}
}
function shopInput(){
	for(i = 1; i < ships.length; i++){ //This is the laziest fix ever. Let me explain why we start at 1.
		//It's so we don't have to include 0 as an input, which our array starts at.
		//However,our KeyDown won't work properly, since the game view shows no 0 variable.
		//So we just pretend 0 doesn't exist, and extend our array arbitrarily. Lazy.
		if(Input.GetKeyDown(i+"")){
			if(GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals >= prices[i]){
				var myShip = Instantiate(ships[i],this.transform.position+Vector3(Random.Range(0,5),Random.Range(0,5),0),Quaternion.identity);
				GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals-=prices[i];
				myShip.transform.parent = null; //Don't set parents, kids!
				this.GetComponent(AudioSource).PlayOneShot(buySound,1);
			}
		}
	}	
}
function checkCanvas(){
	if(selected == true){
		shopCanvas.active = true;
	}
	else{
		shopCanvas.active = false;
	}
}
function clearSelection(){
	selected = false;
}
//LARGE SECTION OF STATION RELATED CODE
function spawnPlayerStation(){//Player has captured this station!
	var myParent = GameObject.FindGameObjectWithTag("Squad Manager").transform.GetChild(2);
	var myStation = Instantiate(playerStation, this.transform.position, this.transform.rotation);
	myStation.transform.parent = myParent.transform;
}
function spawnEnemyStation(){//Enemy has captured this station!
	var myStation = Instantiate(enemyStation, this.transform.position, this.transform.rotation);
	myStation.transform.parent = null;
}
function setProgressUI(){//Update capture graphics!
	var progressBar = this.gameObject.transform.GetChild(0);
	
	if(captureProgress > 0){
		progressBar.GetComponent(SpriteRenderer).color.g = captureProgress/5;
		progressBar.GetComponent(SpriteRenderer).color.r = 0;
		progressBar.GetComponent(SpriteRenderer).color.a = interactTimer/5;
	}
	else{
		if(captureProgress < 0){
			progressBar.GetComponent(SpriteRenderer).color.r = Mathf.Abs(captureProgress)/5;
			progressBar.GetComponent(SpriteRenderer).color.g = 0;
			progressBar.GetComponent(SpriteRenderer).color.a = interactTimer/5;
		}
		else{
			if(progressBar == 0){
				progressBar.GetComponent(SpriteRenderer).color.r = 1;
				progressBar.GetComponent(SpriteRenderer).color.g = 1;
				progressBar.GetComponent(SpriteRenderer).color.b = 1;
				progressBar.GetComponent(SpriteRenderer).color.a = interactTimer/5;
			}
			else{
				progressBar.GetComponent(SpriteRenderer).color.b = 0;
			}
		}
	}
}
function checkInteractTimer(){ //Check time since last capture
	if(interactTimer >= 0){
		interactTimer-=Time.deltaTime;
	}
	else{
		interactTimer = 0;
		captureProgress = 0;
	}
}
function CaptureLogic(){
	if(captureProgress >= captureTime){
		spawnPlayerStation();
		Destroy(this.gameObject);
	}
	else{
		if(captureProgress <= -captureTime){
			spawnEnemyStation();
			Destroy(this.gameObject);
		}
	}
}
function checkSquadSelect(){ //We check if the base SQUAD is selected
	var myParent = this.transform.parent.gameObject.GetComponent(SquadManager);
	if(myParent.squadSelected == true){
		return true;
	}
	else{
		return false;
	}
}
function updateShop(){
	if(shopCanvas == null){
		shopCanvas = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Shop Panel").gameObject;
	}
}
//END MOST OF THE STATION CODE
function Update () { //Called every frame
	if(shipHealth <= 0){
		Die();
	}
	if(myType == 5){ //Station
		checkInteractTimer();
		CaptureLogic();
		setProgressUI();
		if(selected == true){
			shopInput();
			checkCanvas();
			}
			if(checkSquadSelect() == false){
				updateShop();
				shopCanvas.active = false;
			}
		}
	else{
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
}
public function getMaxHealth(){
	return maxHealth;
}
public function getShipHealth(){
	return shipHealth;
}