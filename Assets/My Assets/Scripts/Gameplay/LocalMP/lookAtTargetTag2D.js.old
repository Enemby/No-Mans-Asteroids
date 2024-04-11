var targetTag : String = "Enemy";
var target : GameObject;
function Start(){
InvokeRepeating("findTarget",0.5f,2.0f);
}
function findTarget(){
	var targets = GameObject.FindGameObjectsWithTag(targetTag);
	if(targets.Length > 0){
		target = targets[0];
	}
}
function Update(){
	if(target != null){
		this.transform.up = target.transform.position - transform.position; //Lazy 2D look at
		this.transform.rotation.eulerAngles.x = 0;
	}
}