public var selectedIndex : int = 0;
private var squadSelected : boolean = false;
function checkSelection(){
	if(this.transform.childCount > 0){
		squadSelected = false;
		for(var i = 0;i < this.transform.childCount;i++){
			if(this.transform.GetChild(i).GetComponent("SquadManager").squadSelected == true){
				selectedIndex = i;
				squadSelected = true;
			}
		}
		if(squadSelected == false){ //Nothing is selected! Let's pick something! :D
			selectedIndex = 0;
			this.transform.GetChild(0).GetComponent("SquadManager").squadSelected = true;
		}
	}
}
function switchSelected(){
	if(this.transform.childCount > 0){
		this.transform.GetChild(selectedIndex).GetComponent("SquadManager").squadSelected = false;
		this.transform.GetChild(selectedIndex).GetComponent("SquadManager").BroadcastMessage("clearSelection");
		if(selectedIndex == this.transform.childCount-1){
			selectedIndex=0;
		}
		else{
			if(selectedIndex+1 <= this.transform.childCount-1){
				selectedIndex+=1;
			}
		}
		this.transform.GetChild(selectedIndex).GetComponent("SquadManager").squadSelected = true;
	}
}
function checkEmpty(){
	if(this.transform.GetChild(selectedIndex).transform.childCount <= 0){
		switchSelected();
	}
}
function Update(){
	if(Input.GetButtonDown("SwitchSquad") && squadSelected == true){
		switchSelected();
		checkEmpty();
		checkEmpty();
	}
	if(Time.deltaTime > 0){
		checkSelection();
	}
}