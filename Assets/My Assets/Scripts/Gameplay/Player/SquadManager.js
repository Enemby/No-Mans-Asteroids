var selectedIndex : int = 0;
private var shipSelected : boolean = false;
private var selectedShip : GameObject; //Ship we have selected
var squadSelected : boolean = false;
var basicShip : GameObject;
var allowEmpty : boolean = false; //Should we let the squad be empty?
function Start(){
	InvokeRepeating("respawnCheck",0.0f,4.0);
}
function Update(){	
	if(Input.GetButtonDown("SwitchShip") && squadSelected == true){
		switchSelected();
	}
	if(Time.timeScale > 0){
		if(squadSelected == true && shipSelected == false){ //Enable a ship ONLY if the squad is selected.
			checkSelection();
			setCameraTarget();
		}
		else{
			clearSelection();
		}
	}
}
function respawnCheck(){
	if(this.transform.childCount == 0 && allowEmpty == false){
		respawnSquad();
	}
}
function checkSelection(){
	if(this.transform.childCount > 0){
		if(this.transform.GetChild(selectedIndex) != null){
			this.transform.GetChild(selectedIndex).GetComponent("PlayerShip").selected = true;
			shipSelected = true;
		}
		else{
			selectedIndex = 0;
			this.transform.GetChild(0).GetComponent("PlayerShip").selected = true;
			shipSelected = true;
			/*
			for(var i = 0;i < this.transform.childCount;i++){
				if(this.transform.GetChild(i).GetComponent("PlayerShip").selected == true){
					selectedIndex = i;
					shipSelected = true;
				}
			}
			*/
			
		}
	}
}
function clearSelection(){
	if(this.transform.childCount > 0){
		shipSelected = false;
		for(var i = 0;i < this.transform.childCount;i++){
			this.transform.GetChild(selectedIndex).GetComponent("PlayerShip").selected == false;
			this.transform.GetChild(i).GetComponent("PlayerShip").selected == false;
		}
	}
}
function setCameraTarget(){
	if(this.transform.childCount > 0){
		selectedShip = this.transform.GetChild(selectedIndex).gameObject;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent("Follow Player").target = selectedShip;
	}
}
function switchSelected(){
	if(this.transform.childCount > 0){
		this.transform.GetChild(selectedIndex).GetComponent("PlayerShip").selected = false;
		if(selectedIndex == this.transform.childCount-1){
			selectedIndex=0;
		}
		else{
			if(selectedIndex+1 <= this.transform.childCount-1){
				selectedIndex+=1;
			}
		}
		this.transform.GetChild(selectedIndex).GetComponent("PlayerShip").selected = true;
	}
}
function respawnSquad(){
	var simpleShip = Instantiate(basicShip, this.transform.position,Quaternion.identity);
	simpleShip.transform.parent = this.transform;
}