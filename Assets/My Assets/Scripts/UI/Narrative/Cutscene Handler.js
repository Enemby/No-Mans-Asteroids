var ourSteps : GameObject[];
var interval : int = 0;
var nextScene : String;
var loadingText : GameObject;
var myTimer : float = 0;
function Start(){
	EffectsCheck();
}
function EffectsCheck(){
	if(PlayerPrefs.GetInt("EffectsCheck") == 0){
		GameObject.Find("Main Camera").GetComponent("Tube").enabled = false;
	}
}
function Update(){
	myTimer+=Time.deltaTime;
	if(myTimer >= 2){
		if(Input.anyKeyDown){
			iterateArray();
		}
	}
}
function iterateArray(){
	if(interval+1 < ourSteps.Length){
		interval+=1;
	}
	else{
		var myText = Instantiate(loadingText,this.transform.position,Quaternion.identity);
		myText.transform.SetParent(GameObject.Find("Canvas").transform,false);
		Application.LoadLevel(nextScene);
	}
	ourSteps[interval-1].active = false;
	ourSteps[interval].active = true;
}