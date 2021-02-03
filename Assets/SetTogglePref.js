var myToggle;
var myPref : String;
function Start(){
	myToggle = this.GetComponent("Toggle");
	if(PlayerPrefs.GetInt(myPref) == 1){
		myToggle.isOn = true;
	}
	else{
		myToggle.isOn = false;
	}
}
function Update () {
}