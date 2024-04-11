using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Test : MonoBehaviour
{
    public virtual void Start()
    {
        this.transform.parent = null;
    }

}