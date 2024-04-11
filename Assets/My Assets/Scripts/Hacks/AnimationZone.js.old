//TODO: Add check to stop animation zones activating without a respawn.
var myTag : String = "SelectedShip";
var checkChildren : boolean = false; //Check if theres an object we could be effecting
var checkChildrenObj : GameObject;
var alreadyTriggered : boolean = false;
private var timer : float = 0;
private var timerMax : float = 1.5;
private var childCount : int = 0;
function Update(){ 
	checkTriggers();
	timer+=Time.deltaTime;
	if(timer >= timerMax){
		timer = 0;
		this.GetComponent(Collider2D).enabled = false;
		this.GetComponent(Collider2D).enabled = true;
	}
}

function checkTriggers(){
	if(checkChildren == true){
		if(checkChildrenObj.transform.GetChildCount() <= 0){
			//alreadyTriggered = false;
			Debug.Log("Triggered: " + alreadyTriggered);	
		}
	}
}
function RemoveControl(mybool : boolean,targetObject : GameObject){ //Stop ship from ruining animation
 //Toggle AllowInput
 	if(mybool == true){
 		targetObject.GetComponent("PlayerShip").allowInput = true;
		//alreadyTriggered = true;
 	}
 	else{
 		targetObject.GetComponent("PlayerShip").allowInput = false;
 	}
}
function toggleVisuals(mybool : boolean){
	if(mybool == true){
		this.GetComponent(SpriteRenderer).enabled = true;
	}
	else{
		this.GetComponent(SpriteRenderer).enabled = false;
	}
}
function OnTriggerEnter2D(otherobj : Collider2D){
	if(checkChildren == true){
		if(checkChildrenObj.transform.GetChildCount() > 0){
			if(otherobj.gameObject.tag == myTag){
				if(alreadyTriggered == false){
					toggleVisuals(true);
					
				}
			}
		}
	}
	else{
		if(otherobj.gameObject.tag == myTag){
			toggleVisuals(true);
		}
	}
}
function OnTriggerStay2D(otherobj : Collider2D){
	if(otherobj.gameObject.tag == myTag){
		if(alreadyTriggered == false){
			RemoveControl(false,otherobj.gameObject);
		}
	}
}
function OnTriggerExit2D(otherobj : Collider2D){
	if(otherobj.gameObject.tag == myTag){
		toggleVisuals(false);
		RemoveControl(true,otherobj.gameObject);
		alreadyTriggered = true;
	}
}