using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class TimerTrigger : MonoBehaviour
{
    public float timeToWait;
    private float myTimer;
    public GameObject toggledObject;
    public virtual void Update()
    {
        this.myTimer = this.myTimer + Time.deltaTime;
        if (this.myTimer >= this.timeToWait)
        {
            this.toggledObject.active = true;
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }

    public TimerTrigger()
    {
        this.timeToWait = 10;
    }

}