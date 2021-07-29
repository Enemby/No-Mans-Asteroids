var myObj : GameObject;
var triggeredObject : GameObject;
function FixedUpdate(){
	if(myObj.transform.parent != null){
		triggeredObject.active = true;
	}
}