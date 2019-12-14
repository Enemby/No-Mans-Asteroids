var myTag : String = "SelectedShip";
var checkChildren : boolean = false; //Check if theres an object we could be effecting
var checkChildrenObj : GameObject;
var alreadyTriggered : boolean = false;
var pushSpeed : Vector2;
private var timer : float = 0;
private var timerMax : float = 1.5;
function Update(){ //Hi Cat!
	timer+=Time.deltaTime;
	if(timer >= timerMax){
		timer = 0;
		this.GetComponent(Collider2D).enabled = false;
		this.GetComponent(Collider2D).enabled = true;
	}
	checkTriggers();
	if(checkChildren == true){
		if(checkChildrenObj.transform.GetChildCount() > 0){
			this.GetComponent(Collider2D).enabled = true;
		}
		else{
			this.GetComponent(Collider2D).enabled = false;
		}
	}
}
function checkTriggers(){
	if(alreadyTriggered == false){
		if(checkChildren == true){
			if(checkChildrenObj.transform.GetChildCount() == 0){
				alreadyTriggered = false;
			}
		}
	}
}
function pushTarget(myObj : GameObject){
	if(myObj.GetComponent(Rigidbody2D)){
		myObj.GetComponent(Rigidbody2D).AddForce(pushSpeed);
	}
}
/*
function OnTriggerEnter2D(otherobj : Collider2D){
		if(otherobj.gameObject.tag == myTag){
		}
}
*/
function OnTriggerStay2D(otherobj : Collider2D){
	if(otherobj.gameObject.tag == myTag){
		if(alreadyTriggered == false){
			pushTarget(otherobj.gameObject);
		}
	}
}
function OnTriggerExit2D(otherobj : Collider2D){
	if(otherobj.gameObject.tag == myTag){
		this.GetComponent(Collider2D).enabled = false;
		alreadyTriggered = true;
	}
}