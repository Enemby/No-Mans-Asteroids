var sceneName : String = "menu";
function Update(){
	if(Input.anyKeyDown){
		Application.LoadLevel(sceneName);
	}
}