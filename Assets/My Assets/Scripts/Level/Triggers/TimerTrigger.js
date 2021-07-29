var timeToWait : float = 10;
private var myTimer : float = 0;
var toggledObject : GameObject;
function Update(){
	myTimer+= Time.deltaTime;
	if(myTimer >= timeToWait){
		toggledObject.active = true;
		Destroy(this.gameObject);
	}
}