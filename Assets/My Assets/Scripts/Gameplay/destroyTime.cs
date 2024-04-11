using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class destroyTime : MonoBehaviour
{
    public float destroyTime;
    public virtual void Start()
    {
    }

    public virtual void Update()
    {
        this.destroyTime = this.destroyTime - Time.deltaTime;
        if (this.destroyTime <= 0)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }

    public destroyTime()
    {
        this.destroyTime = 7.5f;
    }

}