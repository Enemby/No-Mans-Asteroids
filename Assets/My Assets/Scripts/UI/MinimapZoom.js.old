var cornerMap: GameObject;
var zoomMap : GameObject;
function Start(){
	getPreferences();
}
function toggleMaps(){
	if(cornerMap.active == true){
		zoomMap.active = true;
		cornerMap.active = false;
	}
	else{
		zoomMap.active =false;
		cornerMap.active = true;
	}
	savePreferences();
}
function Update(){
	if(Input.GetButtonDown("MinimapZoom")){
		toggleMaps();
	}
}
function savePreferences(){
	if(cornerMap.active == false){
		PlayerPrefs.SetInt("Minimap",1);
	}
	else{
		PlayerPrefs.SetInt("Minimap",0);
	}
	PlayerPrefs.Save();
}
function getPreferences(){
	var myPref = PlayerPrefs.GetInt("Minimap");
	if(myPref == 1){
		zoomMap.active = true;
		cornerMap.active = false;
	}
	else{
		zoomMap.active =false;
		cornerMap.active = true;
	}
}