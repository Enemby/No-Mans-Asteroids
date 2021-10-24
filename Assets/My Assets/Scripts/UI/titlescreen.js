var menuPanel : GameObject;
var creditsPanel : GameObject;
var instantActionPanel : GameObject;
var playPanel : GameObject;
var optionsPanel : GameObject;
var levelToLoad : String = "tutorial";
var instantLevelToLoad : String = "instantaction";
var loadingText : GameObject;
//Quick and dirty titlescreen.
function Play(){
	menuPanel.active = false;
	playPanel.active = true;
}
function InstantActionSelect(){
	instantActionPanel.active = true;
	playPanel.active = false;
}
function CampaignPlay(){
	if(PlayerPrefs.GetInt("tutorialCheck") == 1){
		Application.LoadLevel(levelToLoad);
	}
	/*
	else{
		Application.LoadLevel("beginnertutorial");
	}
	*/
}
function Options(){
	optionsPanel.active = true;
	menuPanel.active = false;
}
function Credits(){
	creditsPanel.active = true;
	menuPanel.active = false;
}
function Back(){
	creditsPanel.active = false;
	optionsPanel.active = false;
	instantActionPanel.active = false;
	playPanel.active = false;
	menuPanel.active = true;
}
function Quit(){
	Application.Quit();
}
function AsteroidsUpdate(mycount:String){
	PlayerPrefs.SetInt("IA_ASTEROIDS",int.Parse(mycount));
	PlayerPrefs.Save();
}
function InstantActionStart(){
	loadingText.active = true;
	Application.LoadLevel(instantLevelToLoad);
}
function OldUpdate(){
	if(creditsPanel.active == true){
		if(Input.anyKeyDown){
			creditsPanel.active = false;
			menuPanel.active = true;
		}
	}
	else{
		if(Input.GetKeyDown("1")){
			Application.LoadLevel("main");
		}
		else{
			if(Input.GetKeyDown("2")){
				creditsPanel.active = true;
				menuPanel.active = false;
			}
			else{
				if(Input.GetKeyDown("3")){
					Application.Quit();
				}
			}
		}
	}
}
function LoadLevel(myLevel : String){
	Application.LoadLevel(myLevel);
}
function hideNewbieCheck(){
	var myCheck = GameObject.FindGameObjectWithTag("NewbieCheck");
	myCheck.active = false;
	bypassNewbieCheck();
}
function Start(){
	setInit();
	AsteroidsUpdate("1000"); //This prevents 0 asteroids if the defaults are left alone
}
function bypassNewbieCheck(){
	PlayerPrefs.SetInt("tutorialCheck",1);
	PlayerPrefs.Save();
}
function setInit(){ //check our initial prefs if not set set them.
	if(!PlayerPrefs.HasKey("tutorialCheck")){
		PlayerPrefs.SetInt("tutorialCheck",0);
	}
	if(!PlayerPrefs.HasKey("maxVolume")){
		PlayerPrefs.SetFloat("maxVolume",1);
	}
	if(!PlayerPrefs.HasKey("musicVolume")){
		PlayerPrefs.SetFloat("musicVolume",0.5);
	}
	if(!PlayerPrefs.HasKey("Controls")){
		PlayerPrefs.SetInt("Controls",1);
	}
	if(!PlayerPrefs.HasKey("RoundTimer")){
		PlayerPrefs.SetInt("RoundTimer",1);
	}
	PlayerPrefs.Save();
}