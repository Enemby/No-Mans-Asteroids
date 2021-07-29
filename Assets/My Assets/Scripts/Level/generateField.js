var asteroids : int = 200;
var maxScale : float = 3;
var spawnRange : int = 10000;
var myAsteroid : GameObject;
function Start(){
	Random.seed = System.DateTime.Now.Ticks;
	generateField();
}
function generateField(){ //Goal: generate an asteroid field.
	for(var i = 0; i < asteroids; i++){
		var newAsteroid = Instantiate(myAsteroid,this.transform.position,Quaternion.identity);
		newAsteroid.transform.position = Vector3(Random.Range(-spawnRange,spawnRange),Random.Range(-spawnRange,spawnRange),0);
		var myScale = Random.Range(0.26,maxScale);
		newAsteroid.transform.localScale.x = myScale;
		newAsteroid.transform.localScale.y = myScale;
	}
}