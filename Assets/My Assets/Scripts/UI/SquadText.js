var parentObj : GameObject;
var myText : UI.Text;
var index : int = 0;
function Update(){
	if(parentObj != null){
		index = parentObj.GetComponent("switchSquad").selectedIndex;
	}
	updateText();
}
function updateText(){
	myText.text = "Squad: "+parentObj.transform.GetChild(index).gameObject.name;
}