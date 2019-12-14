var ColorA : Color;
var ColorB : Color;
var blendTime : float = 0.5;
var myText : UI.Text;
function Start(){
	if(myText == null){
		myText = GetComponent(UI.Text);
	}
}
function Update(){
	myText.color = Color.Lerp(ColorA, ColorB, Mathf.PingPong(Time.time*blendTime, 1));
}