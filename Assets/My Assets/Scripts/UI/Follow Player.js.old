var follow : boolean = true;
var movementSpeed : float = 20;
var target : GameObject;
var screenshake : float = 0;
private var velocity : Vector3 = Vector3(0,0,0);
function SmoothLookAt(mytransform,mytarget){
	//We clone "LookAt", but make it according to Time.deltaTime, not framerate.
	//We also smooth out rotation, so the drone doesn't instantly switch targets.
	var targetRotation = Quaternion.LookRotation(mytarget - mytransform.position);       
	mytransform.rotation = Quaternion.Slerp(mytransform.rotation, targetRotation, 5 * Time.deltaTime);
}
function Update () {
	if(target != null){
		var targetPosition : Vector3 = target.transform.TransformPoint(Vector3(0, 5, -40));
		this.transform.position = Vector3.SmoothDamp(this.transform.position,targetPosition,velocity,0.000001);
		//SmoothLookAt(this.transform,target.transform.position+Vector3(0,7.5,0));
		//No smooth look at. We don't need to rotate.
		if(screenshake > 0){
			this.transform.position.x += Random.Range(-2,2);
			this.transform.position.y += Random.Range(-2,2);
		}
	}
	if(screenshake > 0 ){
		screenshake -=Time.deltaTime;
	}
	if(screenshake < 0){
		screenshake = 0;
	}
}
function ScreenShake(){
	screenshake = 0.5;
}