var myPref : String;
var truevalue : int = 1;
var myToggle : UI.Toggle;
function Start () {
	if(myToggle == false){
		myToggle = this.GetComponent("UI.Toggle");
	}
	if(PlayerPrefs.HasKey(myPref)){
		if(PlayerPrefs.GetInt(myPref) == truevalue){
			myToggle.isOn = true;
		}
		else{
			myToggle.isOn = false;
		}
	}
}