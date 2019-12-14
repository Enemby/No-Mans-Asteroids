//AutoType.js that I found on the internet. I've modified it to have a lot more styling options, and be compatiable with Unity5 UI.
@Header("@/Red #/Green $/Gold %/Magenta ¬/Bold /Italics")
var letterPause : float = 0.2;
var word : String;
var progress : int = 0;
var fade : boolean = true;
private var myText : UI.Text;
private var textLength : int = 0;
function Start () {
	myText = GetComponent(UI.Text);
	word= GetComponent(UI.Text).text;
	textLength = myText.text.Length;
	GetComponent(UI.Text).text = "";
	TypeText();
	InvokeRepeating("finishCheck",10,3);
}
function finishCheck(){ //Check if we're done.
	if(progress >= textLength-1){//Time to fade!
		if(progress != -100){
			if(fade == true){
				InvokeRepeating("fadeText",0,0.025);
				progress = -100;
			}
		}
	}
}
function fadeText(){
	myText.color.a -= Time.deltaTime;
	if(myText.color.a <= 0){
		Destroy(this.gameObject);
	}
}
function TypeText () {
 
	var bold : boolean = false; //toggles the style for bold;
	var red : boolean = false; // toggle red
	var green : boolean = false;
	var gold : boolean = false;
	var magenta : boolean = false;
	var italics : boolean = false;
 
	var ignore : boolean = false; //for ignoring special characters that toggle styles
 
	for (var nextletter in word.ToCharArray()) {
 
		switch (nextletter) {
 			
			case "@":
				ignore = true; //make sure this character isn't printed by ignoring it
				red = !red; //toggle red styling
				green = false; //toggle green styling
				magenta = false; //This is so weird shit doesn't happen.
				gold = false;
			break;
			case "#":
				ignore = true; //make sure this character isn't printed by ignoring it
				green = !green; //toggle green styling
				magenta = false; //toggle green styling
				red = false; //This is so weird shit doesn't happen.
				gold = false;
			break;
			case "$":
				ignore = true; //make sure this character isn't printed by ignoring it
				green = false; //toggle green styling
				red = false; //This is so weird shit doesn't happen.
				magenta = false;
				gold = !gold;
			break;
			case "%":
				ignore = true; //make sure this character isn't printed by ignoring it
				green = false; //toggle green styling
				red = false; //This is so weird shit doesn't happen.
				gold = false;
				magenta = !magenta;
			break;
			case "¬":
				ignore = true; //make sure this character isn't printed by ignoring it
				bold = !bold; //toggle bold styling
			break;
			case "/":
				ignore = true; //make sure this character isn't printed by ignoring it
				italics = !italics; //toggle italic styling
			break;
		}
 
 
		var letter : String = nextletter.ToString();
 
		if (!ignore) {
 
			if (bold){
 
				letter = "<b>"+letter+"</b>";
 
			}
			if (italics){
 
				letter = "<i>"+letter+"</i>";
 
			}
			if (red){
 
				letter = "<color=#ff0000>"+letter+"</color>";
 
			}
 			else{
 				if (green){
 					letter = "<color=#00ff00>"+letter+"</color>";
 				}
 				else{
 				
	 				if (gold){
	 					letter = "<color=#ffff00>"+letter+"</color>";
	 				}
	 				else{
	 					if (magenta){
 							letter = "<color=#cc00ff>"+letter+"</color>";
 						}
	 				}
 				}
 			}
			myText.text += letter;
			progress++;
 
		}
		else{
			progress++; //Without this, we are off by one everytime styling is used.
		}
                //make sure the next character isn't ignored
                ignore = false;
		yield WaitForSeconds (letterPause);
	}
}