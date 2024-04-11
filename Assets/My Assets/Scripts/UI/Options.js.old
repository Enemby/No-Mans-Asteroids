var mypref  :String;
var canvasObject : GameObject;
var pauseMenu : GameObject;
function setPrefName(prefset : String){
	mypref = prefset;
}
function setFloatPref(myvalue : float){
	if( mypref != "" && mypref != null){
		PlayerPrefs.SetFloat(mypref,myvalue);
		PlayerPrefs.Save();
	}
}
function setIntPref(myvalue : int){
	if( mypref != "" && mypref != null){
		PlayerPrefs.SetInt(mypref,myvalue);
		PlayerPrefs.Save();
	}
}
function ToggleIntPref(prefset : String){
	if(PlayerPrefs.GetInt(prefset) == 1){
		PlayerPrefs.SetInt(prefset,0);
	}
	else{
		PlayerPrefs.SetInt(prefset,1);
	}
	PlayerPrefs.Save();
}
function closeMenu(){
	canvasObject.active = false;
	pauseMenu.active = true;
}
function openMenu(){
	canvasObject.active = true;
	pauseMenu.active = false;
}
function deleteMenu(){
	Time.timeScale = 1;
	Destroy(canvasObject.gameObject.transform.root.gameObject);
	Destroy(this.gameObject);
}
function quitToTitle(){
	if(Application.loadedLevel != "menu"){
		Application.LoadLevel("menu");
	}
	else{
		Application.Quit();
	}
}