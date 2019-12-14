var timeToWait : float = 50;
var myText : String;
function Update(){
	if(Time.deltaTime != 0){
		timeToWait-=Time.deltaTime;
	}
	if(timeToWait<= 0){
		Destroy(this.gameObject);
	}
	var myInt : int = timeToWait;
	this.GetComponent(UI.Text).text = myText + myInt + "s";
}