using UnityEngine;
using System.Collections;

[System.Serializable]
public class destroyTime : MonoBehaviour
{
    public float destroyT;
    public virtual void Start()
    {
    }

    public virtual void Update()
    {
        this.destroyT = this.destroyT - Time.deltaTime;
        if (this.destroyT <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public destroyTime()
    {
        this.destroyT = 7.5f;
    }

}