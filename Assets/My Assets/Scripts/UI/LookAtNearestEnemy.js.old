var targetEnemies : GameObject[];
var target : GameObject;
function FixedUpdate(){
	targetEnemies = GameObject.FindGameObjectsWithTag("Enemy");
	if(targetEnemies.Length != 0){
		closestTarget(targetEnemies,2000);
		this.GetComponent(SpriteRenderer).color = Color(1,0.1,0.1,0.3);
	}
	else{
		//Hide me!
		this.GetComponent(SpriteRenderer).color = Color(1,1,1,0);
	}
	lookAtTarget();
}
function lookAtTarget(){
	if(target != null){
		this.transform.up = target.transform.position - transform.position; //Lazy 2D look at
		this.transform.rotation.eulerAngles.x = 0;
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