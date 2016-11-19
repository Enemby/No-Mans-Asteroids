var follow : boolean = true;
var movementSpeed : float = 20;
var target : GameObject;
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
	}
}