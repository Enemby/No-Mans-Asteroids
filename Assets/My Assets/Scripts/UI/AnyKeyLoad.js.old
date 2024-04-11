var scenetoload : String;
var toggleAnimation : boolean = false;
var speed : float = 5;
private var timerstate : boolean = true; //true positive, false negative
private var timer : float = 0;
function Update(){
	if(toggleAnimation != false){
		if(timerstate == true){
			timer+=Time.deltaTime*speed;
		}
		else{
			timer-=Time.deltaTime*speed;
		}
		timer = Mathf.Clamp(timer,0,1);
		this.GetComponent(UI.Text).color.a = timer;
		if(timer == 1){
			timerstate = !timerstate;
		}
		else{
			if(timer == 0){
				timerstate = !timerstate;
			}
		}
	}
	if(Input.anyKeyDown){
		Application.LoadLevel(scenetoload);
	}
}