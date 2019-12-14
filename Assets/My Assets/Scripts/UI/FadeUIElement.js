var fade : boolean = false; //false fade out / true fade in
var fadevalue : float = 0;
var targetAlpha : float = 0.2;
var speed : float = 0.5;
function Update(){
	if(fade == true && fadevalue < targetAlpha){
		fadevalue+=Time.deltaTime * speed;
	}
	if(fade == false && fadevalue > 0){
		fadevalue-=Time.deltaTime * speed;
	}
	capFade();
	GetComponent("Graphic").color.a = fadevalue;
}
function vignetteToggle(mode : int){
	if(mode == 1){ //action
		fade = true;
	}
	else{ 
		fade = false;
	}
}
function capFade(){
	if(fadevalue > targetAlpha){
		fadevalue = targetAlpha;
	}
	if(fadevalue < 0){
		fadevalue = 0;
	}
}