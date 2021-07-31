var hitScaleFactor : float = 0.6; //How much smaller should we get on hit?
var minerals : int = 100;
var randomMinerals : boolean = false; //Do we want random amounts?
var maxMinerals : int = 1000; //How much minerals can be in one asteroid?
var myRB : Rigidbody2D;
private var active : boolean = true;
private var percentage : float = 0;
private var startMinerals : float = 0;
function OnCollisionEnter2D(myCol : Collision2D){
	if(myCol.gameObject.tag == "Bullet"||myCol.gameObject.tag == "EnemyBullet"){
		Destroy(myCol.gameObject);
		if(this.gameObject.transform.childCount > 0){
			for(i=0;i < this.gameObject.transform.childCount;i++){
				if(this.gameObject.transform.GetChild(i).gameObject.tag ==  "Ship"||this.gameObject.transform.GetChild(i).gameObject.tag ==  "SelectedShip"){
					this.gameObject.transform.GetChild(i).gameObject.transform.parent = null;
				}
			}
		}
		var smallAsteroid1 = Instantiate(this.gameObject,this.transform.position,Quaternion.identity);
		var smallAsteroid2 = Instantiate(this.gameObject,this.transform.position,Quaternion.identity);
		smallAsteroid1.GetComponent("Asteroid").randomMinerals = false;
		smallAsteroid2.GetComponent("Asteroid").randomMinerals = false;
		smallAsteroid1.transform.localScale = transform.localScale*hitScaleFactor;
		smallAsteroid2.transform.localScale = transform.localScale*hitScaleFactor;
		smallAsteroid1.GetComponent("Asteroid").minerals = minerals*0.5;
		smallAsteroid2.GetComponent("Asteroid").minerals = minerals*0.5;
		Destroy(this.gameObject);
	}
}
function Start(){
	if(this.transform.localScale.x <= 0.25){
		Destroy(this.gameObject);
	}
	if(randomMinerals == true){
		Random.seed = this.transform.localScale.x; //This doesn't matter that much...
		minerals = Random.Range(1,maxMinerals);
	}
	startMinerals = minerals; //So we can calculate percentage
	percentage = startMinerals/minerals;
	percentage-=1;
	percentage = 1-percentage; //Sleepy me did most of this, I promise.
	this.transform.GetChild(0).GetComponent(SpriteRenderer).color.a = 255*percentage;
	if(myRB == null){
		myRB = this.GetComponent("Rigidbody2D");
	}
	InvokeRepeating("updateUI",0.1f,0.5f+Random.Range(1,20)*0.1f);
}
function updateUI(){
 	if(active == true){
		if(Time.deltaTime > 0){ //slow this shit WAY down. This is 53.2% of cpu time without it.

			if(minerals <= 0){
				this.transform.tag = "Asteroid"; //Because otherwise a Harvester may get stuck!
				this.transform.GetChild(0).GetComponent(SpriteRenderer).color.a = 0;
				active = false;
			}
			else{
				this.transform.tag = "MineralAsteroid"; //For harvester AI
			}
			if(myRB.velocity.magnitude >= 5){
				myRB.velocity*=0.9;
			}
		}
	}
	if(Vector3.Distance(this.transform.position,Vector3.zero) >= 5000){
		Destroy(this);
	}
}