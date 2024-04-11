var checkTag : String;
var index : int = 0;
var targetSize : int = 2;
var toggledObject : GameObject;
function FixedUpdate(){
	if(GameObject.FindGameObjectWithTag(checkTag).transform.GetChild(index).childCount >= targetSize){
		toggledObject.active = true;
		Destroy(this.gameObject);
	}
}