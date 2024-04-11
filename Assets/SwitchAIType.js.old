var myValue : int = 0;
var text : String[];
var myUI : UI.Text;
var myPref : String = "defaultPref";
function Start(){
	setText();
}
function switchUp(){
	myValue++;
	loopCheck();
}
function switchDown(){
	myValue--;
	loopCheck();
}
function loopCheck(){
	if( myValue > text.Length-1){
		myValue = 0;
	}
	else{
		if(myValue < 0){
			myValue = text.Length-1;
		}
	}
}
function setText(){
	myUI.text = text[myValue];
	var output : int;
	if(int.TryParse(text[myValue],output)){
		var myInt = int.Parse(text[myValue]);
		PlayerPrefs.SetInt(myPref, myInt);
		PlayerPrefs.Save();
	}
}