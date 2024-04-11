using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.RequireComponent(typeof(AudioSource))]
public partial class MusicManager : MonoBehaviour
{
    public GameObject[] myInstances;
    public int mode; //0 = Ambient, 1 = Tension, 2 = Action
    public GameObject vignette; //Effect so we can tell when there's trouble.
    private int lastmode;
    public virtual void Start()
    {
        this.lastmode = this.mode;
        this.InvokeRepeating("CombatCheck", 10f, 10f); //In ten seconds, run this function, and run it again every 10 seconds.
    }

    public virtual void SwitchInstance() //Switch between each type of music
    {
        this.myInstances[this.lastmode].gameObject.BroadcastMessage("Deactivate");
        this.myInstances[this.mode].gameObject.BroadcastMessage("Activate");
    }

    public virtual void FixedUpdate()
    {
        if (this.lastmode != this.mode)
        {
            this.SwitchInstance();
            this.lastmode = this.mode;
        }
    }

    public virtual GameObject FindClosestEnemy()
    {
        // Find all game objects with tag Enemy
        GameObject[] gos = null;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = this.transform.position;
        // Iterate through them and find the closest one
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public virtual void CombatCheck()
    {
        GameObject enemyDistance = this.FindClosestEnemy();
        if (enemyDistance != null)
        {
            if (Vector3.Distance(this.transform.position, enemyDistance.transform.position) <= 100)
            {
                this.mode = 1; //Action music
                this.vignette.BroadcastMessage("vignetteToggle", this.mode);
            }
            else
            {
                this.mode = 0; //Ambient music
                this.vignette.BroadcastMessage("vignetteToggle", this.mode);
            }
        }
    }

    public MusicManager()
    {
        this.lastmode = this.mode;
    }

}