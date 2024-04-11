using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class lookAtTarget2D : MonoBehaviour
{
    public GameObject target;
    public virtual void Update()
    {
        if (this.target != null)
        {
            this.transform.up = this.target.transform.position - this.transform.position; //Lazy 2D look at
            this.transform.rotation.eulerAngles.x = 0;
        }
    }

}