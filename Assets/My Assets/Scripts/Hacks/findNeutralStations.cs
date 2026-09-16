using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public partial class findNeutralStations : MonoBehaviour
{
    public GameObject[] targetStations;
    public GameObject target;
    private UnityEngine.UI.Image myRenderer;
    public virtual void Start()
    {
        myRenderer = GetComponent<UnityEngine.UI.Image>();
    }

    public virtual void FixedUpdate()
    {
        GameObject[] targetStations = GameObject.FindGameObjectsWithTag("NeutralStation");
        if (targetStations.Length != 0)
        {
            this.closestTarget(targetStations, 2000);
            this.myRenderer.color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            targetStations = GameObject.FindGameObjectsWithTag("EnemyStation");
            if (targetStations.Length != 0)
            {
                this.closestTarget(targetStations, 2000);
                this.myRenderer.color = new Color(1, 0.1f, 0.1f, 0.3f);
            }
            else
            {
                targetStations = GameObject.FindGameObjectsWithTag("Enemy");
                if (targetStations.Length != 0)
                {
                    this.closestTarget(targetStations, 2000);
                    this.myRenderer.color = new Color(1, 0.1f, 0.1f, 0.3f);
                }
                else
                {
                    targetStations = GameObject.FindGameObjectsWithTag("SpikeEnemy");
                    if (targetStations.Length != 0)
                    {
                        this.closestTarget(targetStations, 2000);
                        this.myRenderer.color = new Color(1, 0.1f, 0.1f, 0.3f);
                    }
                    else
                    {
                        //Hide me!
                        this.myRenderer.color = new Color(1, 1, 1, 0);
                    }
                }
            }
        }
        this.lookAtTarget();
    }

    public virtual void lookAtTarget()
    {
        if (this.target != null)
        {
            if (GameObject.FindGameObjectWithTag("SelectedShip") != null)
            {
                this.transform.up = this.target.transform.position - GameObject.FindGameObjectWithTag("SelectedShip").transform.position; //Lazy 2D look at
                Vector3 myEuler = transform.eulerAngles;
                myEuler.x = 0;
                transform.eulerAngles = myEuler;
            }
        }
    }

    public virtual void closestTarget(GameObject[] targets, int lowestDistance)
    {
        //var lowestDistance = 50; //Scan within  radius
        int ourTarget = -50; //Which ship array index?
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