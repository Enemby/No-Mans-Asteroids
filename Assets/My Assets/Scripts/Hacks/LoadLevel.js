var levelToLoad : String;
var loadingText : GameObject;
//TODO: Make the user confirm they want to complete the level before loading.
//this is purely an accesibility thing.
function Start(){
	var myText = Instantiate(loadingText,this.transform.position,Quaternion.identity);
	myText.transform.SetParent(GameObject.Find("Canvas").transform,false);
	Application.LoadLevel(levelToLoad);
}