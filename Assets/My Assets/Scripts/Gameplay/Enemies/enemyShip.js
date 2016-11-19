//Customizable Enemy Ships
@Header("Ship Type")
var myType : ShipType = 2; // 0/Fighter 1/Harvester 2/Kamikazae 3/Carrier 4/Suppressor
var target : GameObject; //Chase Selected Ship!
var speed : float = 200; //Our speed of movement
var health : float = 3; //Our health
var maxSpeed = 400; //Our MAX speed
@Header("BulletVars")
var bullet : GameObject;
var explosionParticle : GameObject;
var cooldown : float = 1;
var fireTimer : float = 0;
var fireForce : float = 500;
@Header("Carrier Vars")
var shipSpawns : GameObject[]; //What ships can we spawn?
var distanceChecks : float[]; //How close the player have to be?
private var myRigidbody : Rigidbody2D;
function Start(){
	myRigidbody = this.GetComponent(Rigidbody2D);
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
function Update () {
	if(Vector3.Distance(this.transform.position,GameObject.FindGameObjectWithTag("SelectedShip").transform.position) <= 500){ //Optimization
		/*
		if(GameObject.FindGameObjectsWithTag("Enemy").Length >= 75 && myType != 3){
			Die(); //Optimization
		}
		*/ //This is dumb
		if(myType == 0){ //Fighter..?
			target = GameObject.FindGameObjectWithTag("SelectedShip");
			moveToFireRange();
			Fire();
		}
		if(myType == 2){ //Kamikaze
			target = GameObject.FindGameObjectWithTag("SelectedShip");
			if(target != null){ //Stop from throwing errors at start.
				//this.transform.LookAt(target.transform);
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
		if(health <= 0){
			Die();
		}
		maintainSpeed();
		weaponTimer();
	}
}
function moveToFireRange(){
	this.transform.up = target.transform.position - transform.position; //Lazy 2D look at
	this.transform.rotation.eulerAngles.x = 0;
	myRigidbody.AddForce(this.transform.up * speed);
	if(Vector3.Distance(this.transform.position,target.transform.position) <= 5){
		myRigidbody.AddForce(-this.transform.up * speed); //The most lazy fix ever. This is how Half Life 2 "Limited" velocity. Except y'know, 2D.
		maintainSpeed();
	}
}
function Fire(){ //Shoot a bullet, No Caching! :(
	if(fireTimer <= 0.01){
		fireTimer = cooldown;
		if(myType == 0){ //Fighter
			var myBullet = Instantiate(bullet,this.transform.position, Quaternion.identity);
			Physics2D.IgnoreCollision(myBullet.GetComponent(BoxCollider2D),this.GetComponent(BoxCollider2D));
			if(myRigidbody.velocity.magnitude > 1){
				myBullet.GetComponent(Rigidbody2D).velocity = myRigidbody.velocity;
			}
			myBullet.GetComponent(Rigidbody2D).AddForce(this.transform.up*fireForce);
		}
		if(myType == 3){ //Carrier!
			
		}
	}
}
function Die(){
	var explosion = Instantiate(explosionParticle,this.transform.position,Quaternion.identity);
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
	}
}