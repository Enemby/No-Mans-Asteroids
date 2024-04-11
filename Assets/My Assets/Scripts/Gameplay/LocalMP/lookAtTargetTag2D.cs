using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class lookAtTargetTag2D : MonoBehaviour
{
    public string targetTag;
    public GameObject target;
    public virtual void Start()
    {
        this.InvokeRepeating("findTarget", 0.5f, 2f);
    }

    public virtual void findTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(this.targetTag);
        if (targets.Length > 0)
        {
            this.target = targets[0];
        }
    }

    public virtual void Update()
    {
        if (this.target != null)
        {
            this.transform.up = this.target.transform.position - this.transform.position; //Lazy 2D look at
            this.transform.rotation.eulerAngles.x = 0;
        }
    }

    public lookAtTargetTag2D()
    {
        this.targetTag = "Enemy";
    }

}