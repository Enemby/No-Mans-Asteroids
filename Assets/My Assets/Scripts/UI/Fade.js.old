//Fades a SpriteRender alpha channel depending on vars set.
var fadeSpeed : float = 0.1;
var fadeIn : boolean = false;
var loadScene : boolean = false;
var sceneName : String = "null";
function Update(){
	if(fadeIn == false){
		this.GetComponent(SpriteRenderer).color.a-=fadeSpeed;
	}
	else{
		this.GetComponent(SpriteRenderer).color.a+=fadeSpeed;
	}
	if(this.GetComponent(SpriteRenderer).color.a <= 0 || this.GetComponent(SpriteRenderer).color.a >= 1){
		if(loadScene == true){
			Application.LoadLevel(sceneName);
		}
		else{
			Destroy(this.gameObject,50); //Make sure we clean up eventually...
		}
	}
}