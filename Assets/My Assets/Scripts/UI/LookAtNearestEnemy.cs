using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class LookAtNearestEnemy : MonoBehaviour
{
    public GameObject[] targetEnemies;
    public GameObject target;
    public virtual void FixedUpdate()
    {
        this.targetEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (this.targetEnemies.Length != 0)
        {
            this.closestTarget(this.targetEnemies, 2000);
            ((SpriteRenderer) this.GetComponent(typeof(SpriteRenderer))).color = new Color(1, 0.1f, 0.1f, 0.3f);
        }
        else
        {
            //Hide me!
            ((SpriteRenderer) this.GetComponent(typeof(SpriteRenderer))).color = new Color(1, 1, 1, 0);
        }
        this.lookAtTarget();
    }

    public virtual void lookAtTarget()
    {
        if (this.target != null)
        {
            this.transform.up = this.target.transform.position - this.transform.position; //Lazy 2D look at
            this.transform.rotation.eulerAngles.x = 0;
        }
    }

    public virtual void closestTarget(GameObject[] targets, int lowestDistance)
    {
        //var lowestDistance = 50; //Scan within  radius
        object ourTarget = null; //Which ship array index?
        int i = 0;
        while (i < targets.Length) //Scan through enemies
        {
            if (Vector2.Distance(targets[i].transform.position, this.transform.position) <= lowestDistance)
            {
                ourTarget = i;
                lowestDistance = (int) Vector2.Distance(targets[i].transform.position, this.transform.position);
            }
            i++;
        }
        if (!(ourTarget == null))
        {
            this.target = targets[ourTarget]; //Target found. Let's move on to the next thing!
        }
    }

}