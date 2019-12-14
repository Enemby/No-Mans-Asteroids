//Check if playerpref is equal to value.
//If so, enable object.
var myPref : String; //Could use support for any kind of pref.
var requestedValue : int = 0;
var targetObject : GameObject;
function Start(){
	if(PlayerPrefs.HasKey(myPref)){
		if(PlayerPrefs.GetInt(myPref) == requestedValue){
			targetObject.active = true;
		}
	}
	else{
		//do nothing
	}
}
function Update(){
	if(PlayerPrefs.GetInt(myPref) != requestedValue){
		targetObject.active = false;
	}
}