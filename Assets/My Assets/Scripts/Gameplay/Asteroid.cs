using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Asteroid : MonoBehaviour
{
    public float hitScaleFactor; //How much smaller should we get on hit?
    public int minerals;
    public bool randomMinerals; //Do we want random amounts?
    public int maxMinerals; //How much minerals can be in one asteroid?
    public Rigidbody2D myRB;
    private bool active;
    private float percentage;
    private float startMinerals;
    public virtual void OnCollisionEnter2D(Collision2D myCol)
    {
        if ((myCol.gameObject.tag == "Bullet") || (myCol.gameObject.tag == "EnemyBullet"))
        {
            UnityEngine.Object.Destroy(myCol.gameObject);
            if (this.gameObject.transform.childCount > 0)
            {
                i = 0;
                while (i < this.gameObject.transform.childCount)
                {
                    if ((this.gameObject.transform.GetChild(i).gameObject.tag == "Ship") || (this.gameObject.transform.GetChild(i).gameObject.tag == "SelectedShip"))
                    {
                        this.gameObject.transform.GetChild(i).gameObject.transform.parent = null;
                    }
                    i++;
                }
            }
            GameObject smallAsteroid1 = UnityEngine.Object.Instantiate(this.gameObject, this.transform.position, Quaternion.identity);
            GameObject smallAsteroid2 = UnityEngine.Object.Instantiate(this.gameObject, this.transform.position, Quaternion.identity);
            smallAsteroid1.GetComponent("Asteroid").randomMinerals = false;
            smallAsteroid2.GetComponent("Asteroid").randomMinerals = false;
            smallAsteroid1.transform.localScale = this.transform.localScale * this.hitScaleFactor;
            smallAsteroid2.transform.localScale = this.transform.localScale * this.hitScaleFactor;
            smallAsteroid1.GetComponent("Asteroid").minerals = this.minerals * 0.5f;
            smallAsteroid2.GetComponent("Asteroid").minerals = this.minerals * 0.5f;
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }

    public virtual void Start()
    {
        if (this.transform.localScale.x <= 0.25f)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
        if (this.randomMinerals == true)
        {
            Random.seed = (int) this.transform.localScale.x; //This doesn't matter that much...
            this.minerals = Random.Range(1, this.maxMinerals);
        }
        this.startMinerals = this.minerals; //So we can calculate percentage
        this.percentage = this.startMinerals / this.minerals;
        this.percentage = this.percentage - 1;
        this.percentage = 1 - this.percentage; //Sleepy me did most of this, I promise.
        ((SpriteRenderer) this.transform.GetChild(0).GetComponent(typeof(SpriteRenderer))).color.a = 255 * this.percentage;
        if (this.myRB == null)
        {
            this.myRB = (Rigidbody2D) this.GetComponent("Rigidbody2D");
        }
        this.InvokeRepeating("updateUI", 0.1f, 0.5f + (Random.Range(1, 20) * 0.1f));
    }

    public virtual void updateUI()
    {
        if (this.active == true)
        {
            if (Time.deltaTime > 0) //slow this shit WAY down. This is 53.2% of cpu time without it.
            {
                if (this.minerals <= 0)
                {
                    this.transform.tag = "Asteroid"; //Because otherwise a Harvester may get stuck!
                    ((SpriteRenderer) this.transform.GetChild(0).GetComponent(typeof(SpriteRenderer))).color.a = 0;
                    this.active = false;
                }
                else
                {
                    this.transform.tag = "MineralAsteroid"; //For harvester AI
                }
                if (this.myRB.velocity.magnitude >= 5)
                {
                    this.myRB.velocity = this.myRB.velocity * 0.9f;
                }
                else
                {
                    this.myRB.velocity = Vector3.zero;
                }
            }
        }
        if (Vector3.Distance(this.transform.position, Vector3.zero) >= 5000)
        {
            UnityEngine.Object.Destroy(this);
        }
    }

    public Asteroid()
    {
        this.hitScaleFactor = 0.6f;
        this.minerals = 100;
        this.maxMinerals = 1000;
        this.active = true;
    }

}