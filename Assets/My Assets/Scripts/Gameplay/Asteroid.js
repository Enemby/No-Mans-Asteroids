var hitScaleFactor : float = 0.6; //How much smaller should we get on hit?
var minerals : int = 100;
var randomMinerals : boolean = false; //Do we want random amounts?
var maxMinerals : int = 1000; //How much minerals can be in one asteroid?
private var percentage : float = 0;
private var startMinerals : float = 0;
function OnCollisionEnter2D(myCol : Collision2D){
	if(myCol.gameObject.tag == "Bullet"){
		Destroy(myCol.gameObject);
		var smallAsteroid1 = Instantiate(this.gameObject,this.transform.position,Quaternion.identity);
		var smallAsteroid2 = Instantiate(this.gameObject,this.transform.position,Quaternion.identity);
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
}
function Update(){
	//How to very poorly get a percentage
	percentage = startMinerals/minerals;
	percentage-=1;
	percentage = 1-percentage; //Sleepy me did most of this, I promise.
	this.transform.GetChild(0).GetComponent(SpriteRenderer).color.a = 255*percentage;
	if(minerals <= 0){
		this.transform.tag = "Asteroid"; //Because otherwise a Harvester may get stuck!
	}
	else{
		this.transform.tag = "MineralAsteroid"; //For harvester AI
	}
}