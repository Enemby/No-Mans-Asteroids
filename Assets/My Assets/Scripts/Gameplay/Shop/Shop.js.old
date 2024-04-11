//Allows us to buy ships
var ships : GameObject[];
var prices : int[];
var shopCanvas : GameObject;
var activeDistance : float = 50;
var shopActive : boolean = false;
var spawnPosition : Vector3;
var buySound : AudioClip;
function checkIfNear(){
	var ship = GameObject.FindGameObjectWithTag("SelectedShip");
	if(ship != null){
		if(Vector3.Distance(this.transform.position, ship.transform.position) < activeDistance){
			shopCanvas.active = true;
			shopActive = true;
		}
		else{
			shopCanvas.active = false;
			shopActive = false;
		}
	}
}
function shopInput(){
	for(i = 1; i < ships.length; i++){ //This is the laziest fix ever. Let me explain why we start at 1.
		//It's so we don't have to include 0 as an input, which our array starts at.
		//However,our KeyDown won't work properly, since the game view shows no 0 variable.
		//So we just pretend 0 doesn't exist, and extend our array arbitrarily. Lazy.
		if(Input.GetKeyDown(i+"")){
			if(GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals >= prices[i]){
				var myShip = Instantiate(ships[i],spawnPosition,Quaternion.identity);
				GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerManager").minerals-=prices[i];
				myShip.transform.parent = GameObject.FindGameObjectWithTag("Squad Manager").transform;
				this.GetComponent(AudioSource).PlayOneShot(buySound,1);
			}
		}
	}	
}
function Update(){
	checkIfNear();
	if(shopActive == true){
		shopInput();
	}
}