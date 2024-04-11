var myPref : String;
var mySlider : UI.Slider;
function Start () {
	if(mySlider == false){
		mySlider = this.GetComponent("UI.Slider");
	}
	if(PlayerPrefs.HasKey(myPref)){
		mySlider.value = PlayerPrefs.GetFloat(myPref);
	}
}