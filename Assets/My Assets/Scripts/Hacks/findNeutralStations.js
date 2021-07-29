var targetStations : GameObject[];
var target : GameObject;
private var myRenderer;
function Start(){
	if(this.GetComponent(SpriteRenderer) != null){
		myRenderer = this.GetComponent(SpriteRenderer);
	}
	else{
		myRenderer = this.GetComponent(UI.Image);
	}
}
function FixedUpdate(){
	var targetStations = GameObject.FindGameObjectsWithTag("NeutralStation");
	if(targetStations.Length != 0){
		closestTarget(targetStations,2000);
		myRenderer.color = Color(1,1,1,0.3);
	}
	else{
		targetStations = GameObject.FindGameObjectsWithTag("EnemyStation");
		if(targetStations.Length != 0){
			closestTarget(targetStations,2000);
			myRenderer.color = Color(1,0.1,0.1,0.3);
		}
		else{
		targetStations = GameObject.FindGameObjectsWithTag("Enemy");
			if(targetStations.Length != 0){
				closestTarget(targetStations,2000);
				myRenderer.color = Color(1,0.1,0.1,0.3);
			}
			else{
				targetStations = GameObject.FindGameObjectsWithTag("SpikeEnemy");
				if(targetStations.Length != 0){
					closestTarget(targetStations,2000);
					myRenderer.color = Color(1,0.1,0.1,0.3);
				}
				else{
					//Hide me!
					myRenderer.color = Color(1,1,1,0);
				}
			}
		}
	}
	lookAtTarget();
}
function lookAtTarget(){
	if(target != null){
		if(GameObject.FindGameObjectWithTag("SelectedShip") != null){
			this.transform.up = target.transform.position - GameObject.FindGameObjectWithTag("SelectedShip").transform.position; //Lazy 2D look at
			this.transform.rotation.eulerAngles.x = 0;
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
					target = targets[ourTarget]; //Target found. Let's move on to the next thing!
				}
}