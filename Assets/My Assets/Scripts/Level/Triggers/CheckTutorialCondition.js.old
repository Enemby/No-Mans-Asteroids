var checkTag : String;
var toggledObject : GameObject;
@Header ("If false, triggers on finding no commander")
var checkExists : boolean = true;

function FixedUpdate(){
	if(checkExists == true){
		if(GameObject.FindGameObjectWithTag(checkTag) != null){
			toggledObject.active = true;
			this.gameObject.active = false;
		}
	}
	else{
		if(GameObject.FindGameObjectWithTag(checkTag) == null){
			toggledObject.active = true;
			this.gameObject.active = false;
		}
	}
}