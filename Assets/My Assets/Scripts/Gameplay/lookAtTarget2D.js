var target : GameObject;
function Update(){
	this.transform.up = target.transform.position - transform.position; //Lazy 2D look at
	this.transform.rotation.eulerAngles.x = 0;
}