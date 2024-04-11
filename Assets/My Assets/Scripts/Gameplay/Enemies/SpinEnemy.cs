using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SpinEnemy : MonoBehaviour
{
    public GameObject target; //Chase Selected Ship!
    public float speed;
    public int maxSpeed;
    public virtual void Update()
    {
    }

    public SpinEnemy()
    {
        this.speed = 200;
        this.maxSpeed = 400;
    }

}