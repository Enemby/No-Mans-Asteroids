using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Follow Player : MonoBehaviour
{
    public bool follow;
    public float movementSpeed;
    public GameObject target;
    public float screenshake;
    private Vector3 velocity;
    public virtual void SmoothLookAt(object mytransform, object mytarget)
    {
        //We clone "LookAt", but make it according to Time.deltaTime, not framerate.
        //We also smooth out rotation, so the drone doesn't instantly switch targets.
        Quaternion targetRotation = Quaternion.LookRotation(((object) mytarget) - mytransform.position);
        mytransform.rotation = Quaternion.Slerp(mytransform.rotation, targetRotation, 5 * Time.deltaTime);
    }

    public virtual void Update()
    {
        if (this.target != null)
        {
            Vector3 targetPosition = this.target.transform.TransformPoint(new Vector3(0, 5, -40));
            this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref this.velocity, 1E-06f);
            //SmoothLookAt(this.transform,target.transform.position+Vector3(0,7.5,0));
            //No smooth look at. We don't need to rotate.
            if (this.screenshake > 0)
            {
                this.transform.position.x = this.transform.position.x + Random.Range(-2, 2);
                this.transform.position.y = this.transform.position.y + Random.Range(-2, 2);
            }
        }
        if (this.screenshake > 0)
        {
            this.screenshake = this.screenshake - Time.deltaTime;
        }
        if (this.screenshake < 0)
        {
            this.screenshake = 0;
        }
    }

    public virtual void ScreenShake()
    {
        this.screenshake = 0.5f;
    }

    public Follow Player()
    {
        this.follow = true;
        this.movementSpeed = 20;
        this.velocity = new Vector3(0, 0, 0);
    }

}