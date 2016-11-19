var selectedIndex : int = 0;
private var shipSelected : boolean = false;
private var selectedShip : GameObject; //Ship we have selected
var basicFighter : GameObject;
var basicHarvester : GameObject;
function Update(){
	if(Input.GetButtonDown("SwitchShip")){
		switchSelected();
	}
	if(Time.deltaTime > 0){
		checkSelection();
		setCameraTarget(); //Disable this if you don't want camera to switch targets
	}
	if(this.transform.childCount == 0){
		respawnSquad();
	}
}
function checkSelection(){
	if(this.transform.childCount > 0){
		shipSelected = false;
		for(var i = 0;i < this.transform.childCount;i++){
			if(this.transform.GetChild(i).GetComponent("PlayerShip").selected == true){
				selectedIndex = i;
				shipSelected = true;
			}
		}
		if(shipSelected == false){ //Nothing is selected! Let's pick something! :D
			selectedIndex = 0;
			this.transform.GetChild(0).GetComponent("PlayerShip").selected = true;
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
	var fighter = Instantiate(basicFighter, this.transform.position,Quaternion.identity);
	var harvest = Instantiate(basicHarvester, this.transform.position,Quaternion.identity);
	fighter.transform.parent = this.transform;
	harvest.transform.parent = this.transform;
}