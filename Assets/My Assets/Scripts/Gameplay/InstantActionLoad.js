var neutralStation : GameObject;
var player : GameObject;
var enemyShip : GameObject;
var spawnDistance : float = 400;
var localMP : boolean = false;
private var asteroidGen : GameObject;
private var eCommander : GameObject;
private var stationCount;
function setReferences(){ //Update our references (this saves frames)
	asteroidGen = GameObject.FindGameObjectWithTag("AsteroidGen");
	eCommander = GameObject.FindGameObjectWithTag("EAICOM");
}
function setSettings(){ //Set Instant Action Preferences from the menu
	asteroidGen.GetComponent("generateField").asteroids = PlayerPrefs.GetInt("IA_ASTEROIDS");
	asteroidGen.GetComponent("generateField").spawnRange = PlayerPrefs.GetInt("IA_DENSITY");
	if(eCommander){
		eCommander.GetComponent("AICommander").myType = PlayerPrefs.GetInt("IA_AITYPE");
	}
	stationCount = PlayerPrefs.GetInt("IA_STATIONS");
}
function Awake(){//On scene load, do this:
	setReferences();
	setSettings();
	if(localMP == false){
		spawnMap();
	}
}
function spawnMap(){
	for(i = 0;i< stationCount-1;i++){
		if(i ==0){
			player.transform.position = Vector3(-20,0,0);
		}
		Instantiate(neutralStation,Vector3(i*spawnDistance,0,0),Quaternion.identity);
		if(i >= stationCount-2){
			enemyShip.transform.position = Vector3(i*spawnDistance+10,0,0);
		}
	}
}