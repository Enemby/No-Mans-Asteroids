//Check health percentage, do stuff.
var noiseScript : MonoBehaviour;
var glitchScript : MonoBehaviour;
var maxHealth;
var currentHealth;
var glitchTime : float = 1;
var stayTime : float = 0.2;
private var timer : float = 0;
function UpdateReferences(){
	if(GameObject.FindGameObjectWithTag("SelectedShip") != null){
		var selectedShip = GameObject.FindGameObjectWithTag("SelectedShip").GetComponent("PlayerShip");
		maxHealth = selectedShip.maxHealth;
		currentHealth = selectedShip.shipHealth;
	}
}
function timerUpdate(){
	timer+=Time.deltaTime;
	if(timer > glitchTime && timer < glitchTime+stayTime){
		setGlitch(true);
	}
	else{
		setGlitch(false);
	}
	if(timer > glitchTime+stayTime){
		timer = 0;
	}
	
}
function calculatePercentage(){
	if(currentHealth != null && maxHealth != null){
		var percentage = currentHealth / maxHealth;
		percentage *= 100;
		return percentage;
	}
	else{
		return 100;
	}
}
function setGlitch(myMode : boolean){
	if(myMode == true){
		glitchScript.colorDrift = 0.5;
		glitchScript.scanLineJitter = 0.3;
	}
	else{
		glitchScript.colorDrift = 0;
		glitchScript.scanLineJitter = 0;
	}
}
function setNoiseIntensity(){
	if(calculatePercentage() == 100){
		noiseScript.grainIntensityMax = 0;
		noiseScript.scratchIntensityMax = 0;
	}
	else{ //This one problem, took me a full hour to figure out. (With help)
		noiseScript.grainIntensityMax = 100/calculatePercentage()*0.6;
		noiseScript.scratchIntensityMax = 10/calculatePercentage();
	}
}
function EffectsCheck(myval : boolean){
	if(myval == true){
		noiseScript.enabled = true;
		glitchScript.enabled = true;
	}
	else{
		noiseScript.enabled = false;
		glitchScript.enabled = false;
	}
}
function Update(){
	UpdateReferences();
	if(calculatePercentage() <= 50){
		timerUpdate();
	}
	setNoiseIntensity();
	if(PlayerPrefs.GetInt("EffectsCheck") == 1){
		EffectsCheck(true);
	}
	else{
		EffectsCheck(false);
	}
}