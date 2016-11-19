var menuPanel : GameObject;
var creditsPanel : GameObject;
//Quick and dirty titlescreen.
function Update(){
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