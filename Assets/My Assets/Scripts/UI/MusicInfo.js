var ColorA : Color;
var ColorB : Color;
var blendTime : float = 0;
var myText : UI.Text;
var fade : boolean = false;
var timer : float = 1;
function startPopUp(){
	fade = true;
}
function UpdateText(){
	ColorA.a = timer;
	ColorB.a = timer;
	myText.color = Color.Lerp(ColorA, ColorB, Mathf.PingPong(Time.time*blendTime, 1));
	timer+=Time.deltaTime*0.5;
	if(timer > 1){
		timer = 1;
	}
	if(timer == 1){
		fade = false;
	}
}
function endPopUp(){
	ColorA.a = timer;
	ColorB.a = timer;
	myText.color = Color.Lerp(ColorA, ColorB, Mathf.PingPong(Time.time*blendTime, 1));
	timer-=Time.deltaTime*0.5;
	if(timer < 0){
		timer = 0;
	}
	if(timer == 0){
		fade = false;
	}
}
function Update(){
	if(fade == true){
		UpdateText();
	}
	else{
		endPopUp();
	}
}