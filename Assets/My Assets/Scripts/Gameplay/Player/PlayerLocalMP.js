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
var fireForce : float = 100; //How fast does our bullet move?
var explosionParticle : GameObject; //Explosion effect when we die :(
private var fireTimer : float = 0; //Cooldown timer. You shouldn't have to screw with this unless something goes terribly wrong!
public var shipHealth : float = maxHealth; //current Health. This should stay private, to help stop weird health modification bugs
var target : GameObject;
function Start(){
	myRigidbody = this.GetComponent(Rigidbody2D);
	shipHealth = maxHealth;
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
function Fire(){ //Shoot a bullet, or catch an Asteroid in a Harvester. No Caching! :(
	if(fireTimer <= 0.01){
		fireTimer = cooldown;
		if(myType == 0){ //Fighter
			var myBullet = Instantiate(bullet,this.transform.position, Quaternion.identity);
			Physics2D.IgnoreCollision(myBullet.GetComponent(BoxCollider2D),this.GetComponent(BoxCollider2D));
			myBullet.GetComponent(Rigidbody2D).velocity = this.transform.up*fireForce;
			myBullet.GetComponent(Rigidbody2D).velocity += this.GetComponent(Rigidbody2D).velocity; //0.5 is more correct, but makes you feel like you shot at an angle at high speeds.
			this.GetComponent(AudioSource).PlayOneShot(fireSound, 0.5);
		}
	}
}
function shipInput(){ //Check for button presses, act accordingly.
	//Movement
	if(allowInput == true){
		this.transform.localRotation.eulerAngles.z -= Input.GetAxisRaw("HorizontalP2") * turnSpeed *Time.deltaTime*14;
		myRigidbody.AddForce(this.transform.up*Input.GetAxisRaw("VerticalP2")*speed); //2D physics
		if(Input.GetButton("SlowP2")){
			myRigidbody.velocity*=0.9;
		}
		if(Input.GetButton("FireP2")){
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
function OnCollisionEnter2D(mycol : Collision2D){ //Check if we're being hurt/shot at
	if(mycol.collider.tag == "SpikeEnemy"){
		shipHealth-=1;
	}
	if(mycol.collider.tag == "Bullet"){
		shipHealth-=0.5;
		if(selected == true){
			GameObject.FindGameObjectWithTag("MainCamera").BroadcastMessage("ScreenShake");
		}
		Destroy(mycol.gameObject);
	}
}
function clearSelection(){
	selected = false;
}
function Update () { //Called every frame
	if(shipHealth <= 0){
		Die();
	}
	if(selected == true){ //Are we controlling this ship?
		myRigidbody.isKinematic = false;
		shipInput();
		this.transform.tag = "PlayerLocalMP";
	}
	weaponTimer();
}
public function getMaxHealth(){
	return maxHealth;
}
public function getShipHealth(){
	return shipHealth;
}