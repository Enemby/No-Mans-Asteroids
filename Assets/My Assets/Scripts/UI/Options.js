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
function closeMenu(){
	canvasObject.active = false;
	pauseMenu.active = true;
}
function openMenu(){
	canvasObject.active = true;
	pauseMenu.active = false;
}
function deleteMenu(){
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