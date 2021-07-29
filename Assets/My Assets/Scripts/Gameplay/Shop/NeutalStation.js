var captureProgress : float = 0;
var captureTime : float = 5;
var playerStation : GameObject;
var neutralStation : GameObject;
var enemyStation : GameObject;
var interactTimer : float = 0; //Decrease capture after certain time
//This script creates a neutral space station, that can be captured by either team.
function Update(){
	checkInteractTimer();
	CaptureLogic();
	setProgressUI();
}
function spawnPlayerStation(){//Player has captured this station!
	var myParent = GameObject.FindGameObjectWithTag("Squad Manager").transform.GetChild(2);
	var myStation = Instantiate(playerStation, this.transform.position, this.transform.rotation);
	myStation.transform.parent = myParent.transform;
}
function spawnEnemyStation(){//Enemy has captured this station!
	var myStation = Instantiate(enemyStation, this.transform.position, this.transform.rotation);
	myStation.transform.parent = null;
}
function setProgressUI(){//Update capture graphics!
	var progressBar = this.gameObject.transform.GetChild(0);
	
	if(captureProgress > 0){
		progressBar.GetComponent(SpriteRenderer).color.g = captureProgress/5;
		progressBar.GetComponent(SpriteRenderer).color.r = 0;
		progressBar.GetComponent(SpriteRenderer).color.a = interactTimer/5;
	}
	else{
		if(captureProgress < 0){
			progressBar.GetComponent(SpriteRenderer).color.r = Mathf.Abs(captureProgress)/5;
			progressBar.GetComponent(SpriteRenderer).color.g = 0;
			progressBar.GetComponent(SpriteRenderer).color.a = interactTimer/5;
		}
		else{
			if(progressBar == 0){
				progressBar.GetComponent(SpriteRenderer).color.r = 1;
				progressBar.GetComponent(SpriteRenderer).color.g = 1;
				progressBar.GetComponent(SpriteRenderer).color.b = 1;
				progressBar.GetComponent(SpriteRenderer).color.a = interactTimer/5;
			}
			else{
				progressBar.GetComponent(SpriteRenderer).color.b = 0;
			}
		}
	}
}
function checkInteractTimer(){ //Check time since last capture
	if(interactTimer >= 0){
		interactTimer-=Time.deltaTime;
	}
	else{
		interactTimer = 0;
		captureProgress = 0;
	}
}
function CaptureLogic(){
	if(captureProgress >= captureTime){
		spawnPlayerStation();
		Destroy(this.gameObject);
	}
	else{
		if(captureProgress <= -captureTime){
			spawnEnemyStation();
			Destroy(this.gameObject);
		}
	}
}
function OnCollisionStay2D(otherobj : Collision2D){//Check if any ships are touching me!
	if(otherobj.gameObject.tag == "Ship"||otherobj.gameObject.tag == "SelectedShip"){//Player Capture!
		captureProgress +=Time.deltaTime;
		interactTimer = 5;
	}
	else{
		if(otherobj.gameObject.tag == "Enemy"){ //Enemy Capture!
			captureProgress-=Time.deltaTime;
			interactTimer = 5;
		}
	}
}