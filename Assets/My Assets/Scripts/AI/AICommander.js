var myType : CommanderType = 0; //0-defense 1-offense 2-balanced
var myMinerals : float = 100;
var myFighters : GameObject[];//Resource prices go up by 100 for ship type
var myCurrentShips : GameObject[];
var thinkTimer : float = 0.5; //How often should the AI send orders?
var agroTime: float = 0; //How long have we been waiting for something to happen?
var attacked : boolean = false; //Are we provoked!
private var internalClock : float = 0; //Checking time differences.
private var maxShips : int = 10;
var debugTarget : GameObject = null;
private var myStations : GameObject[];
private var playerStations : GameObject[];
private var neutralStations : GameObject[];
function typeMaxShips(){//Set max ships according to commander type
	//TODO: Change with Commander Types.
	maxShips = 7;
}
function Start(){
	typeMaxShips();
}
function updateReferences(){ //Update the vars used in other functions
	myStations = GameObject.FindGameObjectsWithTag("EnemyStation");
	neutralStations = GameObject.FindGameObjectsWithTag("NeutralStation");
	myCurrentShips = GameObject.FindGameObjectsWithTag("Enemy")+GameObject.FindGameObjectsWithTag("SpikeEnemy");
	updatePlayerStations();
}
function updatePlayerStations(){ //This solution is so stupid it I'm giving it a seperate function to celebrate.
	var maxCount = GameObject.Find("Base").transform.childCount;
	if(maxCount > 0){
		var myPStations = GameObject.Find("Base").GetComponentsInChildren(Transform);
		playerStations = new GameObject[GameObject.Find("Base").transform.childCount];
		var actualStations : int = 0;
		for(i=0;i<myPStations.Length-1;i++){
			if(myPStations[i].gameObject.name == "Player Station(Clone)"||myPStations[i].gameObject.name == "Player Station"){
				playerStations[actualStations] = myPStations[i].gameObject;
				actualStations+=1;
			}
		};
	}	
}
function DefendStation(){
	if(myStations.Length == 1){
		for(i = 0;i<myCurrentShips.Length;i++){
			myObj = new GameObject();
			myObj.transform.position = myStations[0].transform.position + Random.insideUnitCircle * 200;
			setShipTarget(myCurrentShips[i],myObj);
			Destroy(myObj,25);
		}
	}
}
function closestTarget(targets : GameObject[],lowestDistance : int){
	//var lowestDistance = 50; //Scan within  radius
				var ourTarget; //Which ship array index?
				for(var i = 0; i < targets.length; i++){ //Scan through enemies
					if(Vector2.Distance(targets[i].transform.position,this.transform.position) <= lowestDistance){
						ourTarget = i;
						lowestDistance = Vector2.Distance(targets[i].transform.position,this.transform.position);
					}
				}
				if(ourTarget != null){
					return targets[ourTarget]; //Target found. Let's move on to the next thing!
				}
}
function collectResources(){ //simplified, for optimization
	if(GameObject.FindGameObjectsWithTag("EnemyStation").Length > 0){
		var addedMinerals = Time.deltaTime+GameObject.FindGameObjectsWithTag("EnemyStation").Length;
		addedMinerals*=0.5; //Adjust mining speed as needed
		myMinerals+=addedMinerals;
	}
}
function setShipTarget(theShip : GameObject,theTarget : GameObject){
	if(theShip.transform.tag == "Enemy"||theShip.transform.tag == "SpikeEnemy"){//Cool, not a garbage var.
		theShip.GetComponent("enemyShip").target = theTarget;
	}
}
function DesperationCheck(){//Assess Resources, make a plan.
	if(myStations.Length < 1){ //Oh shit!
		//Debug.Log("No EStations!");
		if(neutralStations.Length > 0){//Well, that's a start.
			//Debug.Log("Found NStations!");
			if(GameObject.FindGameObjectsWithTag("Enemy").Length > 0){//We have options!
				//Debug.Log("Found Ships!");
				var everyShip = myCurrentShips;
				var targetStation = closestTarget(neutralStations,5000);
				for(i=0;i<everyShip.Length;i++){
					setShipTarget(everyShip[i],targetStation);
				}
			}
		}
	}
	updateReferences();
	if(myStations.Length <= 0){
		if(neutralStations.Length <= 0){
			if(myCurrentShips.Length <= 0){
				Destroy(this.gameObject,5);
			}
			else{
				for(i=0;i<myCurrentShips.Length;i++){
					if(playerStations.Length <= 0){
						setShipTarget(myCurrentShips[i],GameObject.FindGameObjectWithTag("SelectedShip"));
					}
				}
			}
		}
	}
}
function buyShip(myIndex : int){
	//Calculate price
	var cost = myIndex * 100;
	if(myMinerals >= cost){
		if(GameObject.FindGameObjectWithTag("EnemyStation") != null){ //avoid errors on loss
			Instantiate(myFighters[myIndex],GameObject.FindGameObjectWithTag("EnemyStation").transform.position+Vector3(Random.Range(0,20),Random.Range(0,20),0),Quaternion.identity);
			myMinerals-=cost;
		}
	}
}
function supplyShips(){
	if(myCurrentShips.Length < maxShips){ //Make sure we don't exceed max.
		if(myMinerals >= myFighters.Length*100 && myCurrentShips.Length-1 < maxShips){
			buyShip(myFighters.Length-1);
		}
		else{
			//TODO: Algorithm to cost-analyze buying ships.
		}
	}
}
function agroCheck(){
	agroTime+=Time.deltaTime;
	if(myType == 0){ //Defense
		if(agroTime >= 200){
			attacked = true;
			agroTime=0;
		}
	}
	else{
		if(myType == 1){ //Offensive
			if(agroTime >= 50){
				attacked = true;
				agroTime=0;
			}
		}
		else{
			if(myType == 2){ //Balanced
				if(agroTime >= 100){
					attacked = true;
					agroTime=0;
				}
			}
		}
	}
	if(attacked == true){
		if(myCurrentShips.Length < maxShips*0.5){
			attacked = false; //Call off the attack.
			agroTime = 0;
		}
	}
}
function attackPlayerStation(){
	if(playerStations.Length >= 1){
		for(i=0;i < myCurrentShips.Length-myStations.Length-1;i++){//Leave one ship out of the attack.
			setShipTarget(myCurrentShips[i].gameObject,playerStations[0].gameObject);
		}
		if(myStations.Length > 0){
			setShipTarget(myCurrentShips[myCurrentShips.Length-1].gameObject,myStations[0].gameObject);
		}
		else{
			setShipTarget(myCurrentShips[myCurrentShips.Length-1].gameObject,playerStations[0].gameObject);
		}
	}
}
function captureNeutralStation(){
	for(i=0;i < myCurrentShips.Length-myStations.Length-1;i++){//Leave one ship out of the attack.
			setShipTarget(myCurrentShips[i].gameObject,neutralStations[0].gameObject);
		}
		setShipTarget(myCurrentShips[myCurrentShips.Length-1].gameObject,myStations[0].gameObject);
}
function balancedAttack(){
	if(playerStations.Length >= 1 && neutralStations.Length >= 1){
		for(i=0;i < myCurrentShips.Length*0.5;i++){ //attack PlayerStation
			setShipTarget(myCurrentShips[i].gameObject,playerStations[0].gameObject);
		}
		for(i=myCurrentShips.Length*.5;i < myCurrentShips.Length-2;i++){ //attack PlayerStation
			setShipTarget(myCurrentShips[i].gameObject,neutralStations[0].gameObject);
		}
		//We don't set any targets for the last two ships. It means they will DefendStation() by default.
		//setShipTarget(myCurrentShips[myCurrentShips.Length-1].gameObject,myStations[0].gameObject);
	}
}
function attackedCommanderAction(){
	if(myType == 0){ //Defense
		if(neutralStations.Length <= 0){
				captureNeutralStation();
			}
			else{
				attackPlayerStation();
			}
	}
	else{
		if(myType == 1){ //Offensive
			if(playerStations.Length >= 1){
					attackPlayerStation();					
				}
				else{
					captureNeutralStation();
				}
		}
		else{
			if(myType == 2){ //Balanced
				balancedAttack();
			}
		}
	}
	
}
function Update(){
	agroCheck();
	collectResources();
	updateReferences();
	if(internalClock < thinkTimer){
		internalClock+=Time.deltaTime;
	}
	else{
		internalClock = 0;
	}
	if(internalClock <= 0.03){
		if(myType == 0){ //Defensive
			DesperationCheck();
			DefendStation();
		}
		else{
			if(myType == 1){//Offensive
				DesperationCheck();
			}
			else{
				if(myType == 2){//Balanced 
					DesperationCheck();
					DefendStation();
				}
			}
		}
		
		supplyShips();
		
	}
	if(attacked == true){
		attackedCommanderAction(); //Response is different for different commanders.
	}
}