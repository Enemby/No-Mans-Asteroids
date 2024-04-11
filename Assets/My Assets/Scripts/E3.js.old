//run this script on the "E3" gameobject. This will trigger the game ending.
var endScreen : GameObject;
var ourSound : AudioClip;
private var mySource : AudioSource;
function Start(){
	mySource = this.GetComponent(AudioSource);
}
function OnTriggerEnter2D(myobj : Collider2D){
	if(myobj.transform.tag == "SelectedShip"){
		mySource.clip = ourSound;
		mySource.Play();
		var endGO = Instantiate(endScreen, this.transform.position, Quaternion.identity);
	}
}