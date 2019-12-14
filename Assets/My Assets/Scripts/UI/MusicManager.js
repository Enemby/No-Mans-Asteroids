@script RequireComponent(AudioSource)
var myInstances : GameObject[];
var mode : int = 0; //0 = Ambient, 1 = Tension, 2 = Action
var vignette : GameObject; //Effect so we can tell when there's trouble.
private var lastmode : int = mode;
function Start(){
	lastmode = mode;
	InvokeRepeating("CombatCheck",10.0f,10.0f); //In ten seconds, run this function, and run it again every 10 seconds.
}
function SwitchInstance(){ //Switch between each type of music
	myInstances[lastmode].gameObject.BroadcastMessage("Deactivate");
	myInstances[mode].gameObject.BroadcastMessage("Activate");
}
function FixedUpdate(){
	if(lastmode != mode){
		SwitchInstance();
		lastmode = mode;
	}
}
function FindClosestEnemy () : GameObject {
    // Find all game objects with tag Enemy
    var gos : GameObject[];
    gos = GameObject.FindGameObjectsWithTag("Enemy");
    var closest : GameObject;
    var distance = Mathf.Infinity;
    var position = transform.position;
    // Iterate through them and find the closest one
    for (var go : GameObject in gos)  {
        var diff = (go.transform.position - position);
        var curDistance = diff.sqrMagnitude;
        if (curDistance < distance) {
            closest = go;
            distance = curDistance;
        }
    }
    return closest;
}
function CombatCheck(){
	var enemyDistance = FindClosestEnemy();
	if(enemyDistance != null){
		if(Vector3.Distance(this.transform.position,enemyDistance.transform.position) <= 100){
			mode = 1; //Action music
			vignette.BroadcastMessage("vignetteToggle",mode);
		}
		else{
			mode = 0; //Ambient music
			vignette.BroadcastMessage("vignetteToggle",mode);
		}
	}
}