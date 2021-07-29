//Manages all of our basic game variables and UI.
var minerals : int = 0; //How much money do we got, homie?
var player : GameObject; //Who are we looking at, cuz?
var mineralsUI : UI.Text;
var controlsUI : GameObject;
var aiUI : UI.Text;
var myAI : AIMode = 0;
var squadUI : UI.Text;
//var myOptions :GameObject;
function Start(){
	if(PlayerPrefs.HasKey("Minerals") == false){ //Yo, you got that written down dawg?
		PlayerPrefs.SetInt("Minerals",0);
		PlayerPrefs.Save(); //Ya boy, that's how we do.
	}
	else{ //What it say tho
		minerals = PlayerPrefs.GetInt("Minerals"); //I hear you homie
	}
	setAIUI();
}
function setMineralUI(){
	mineralsUI.text = "Minerals: "+minerals;
}
function setAIUI(){ //Set our AI State UI.
	var myText = "AI: ";
	if(myAI == 0){
		myText+="Sentry";
	}
	else{
		if(myAI == 1){
			myText+="Follow";
		}
		else{
			if(myAI == 2){
				myText+="Drift";
			}
		}
	}
	aiUI.text = myText;
}
function Update(){
	setMineralUI();
	if(Input.GetKeyDown("/")){
		Application.Quit();
	}
	if(Input.GetButtonDown("Menu") && GameObject.FindGameObjectWithTag("PauseMenu") == null){
		var async : AsyncOperation = Application.LoadLevelAdditiveAsync ("options");
		//yield async;
		Debug.Log ("Loading complete");
	}
	if(Input.GetButtonDown("AIMode")){
		myAI+=1;
		if(myAI > 2){
			myAI = 0;
		}
		setAIUI();
	}
	if(Input.GetButtonDown("Controls")){
		if(controlsUI.active == true){
			controlsUI.active = false;
			PlayerPrefs.SetInt("Controls",0);
		}
		else{
			controlsUI.active = true;
			PlayerPrefs.SetInt("Controls",1);
		}
		PlayerPrefs.Save();
	}
}
