var levelToLoad : String;
var loadingText : GameObject;
var winningText : GameObject;
function Start(){
	if(winningText != null){
		var myText1 = Instantiate(winningText,this.transform.position,Quaternion.identity);
		myText1.transform.SetParent(GameObject.Find("Canvas").transform,false);
		if(PlayerPrefs.HasKey("RoundTimer") && PlayerPrefs.GetInt("RoundTimer") == 1){
			myText1.transform.GetChild(0).GetComponent("Text").text += Mathf.RoundToInt(Time.timeSinceLevelLoad) + "s";
		}
		else{
			myText1.transform.GetChild(0).GetComponent("Text").text += Time.timeSinceLevelLoad + "s";
		}
		}
}
function Update(){
	if(Input.GetButtonDown("Fire")){
		Load();
	}
}
function Load(){
	var myText = Instantiate(loadingText,this.transform.position,Quaternion.identity);
	myText.transform.SetParent(GameObject.Find("Canvas").transform,false);
	Application.LoadLevel(levelToLoad);
}