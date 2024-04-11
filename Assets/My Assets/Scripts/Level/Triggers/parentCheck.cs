using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class parentCheck : MonoBehaviour
{
    public GameObject myObj;
    public GameObject triggeredObject;
    public virtual void FixedUpdate()
    {
        if (this.myObj.transform.parent != null)
        {
            this.triggeredObject.active = true;
        }
    }

}