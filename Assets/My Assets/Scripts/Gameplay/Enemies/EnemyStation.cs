using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyStation : MonoBehaviour
{
    public float captureProgress;
    public float captureTime;
    public GameObject playerStation;
    public GameObject neutralStation;
    public GameObject enemyStation;
    public float interactTimer; //Decrease capture after certain time
    //This script creates a space station, that can be captured by either team.
    public virtual void Update()
    {
        this.checkInteractTimer();
        this.CaptureLogic();
        this.setProgressUI();
    }

    public virtual void Start()
    {
        if (this.transform.tag != "EnemyStation")
        {
            this.transform.tag = "EnemyStation";
        }
    }

    public virtual void spawnPlayerStation()//Player has captured this station!
    {
        Transform myParent = GameObject.FindGameObjectWithTag("Squad Manager").transform.GetChild(2);
        GameObject myStation = UnityEngine.Object.Instantiate(this.playerStation, this.transform.position, this.transform.rotation);
        myStation.transform.parent = myParent.transform;
    }

    public virtual void spawnEnemyStation()//Enemy has captured this station!
    {
        GameObject myStation = UnityEngine.Object.Instantiate(this.enemyStation, this.transform.position, this.transform.rotation);
        myStation.transform.parent = null;
    }

    public virtual void setProgressUI()//Update capture graphics!
    {
        Transform progressBar = this.gameObject.transform.GetChild(0);
        if (this.captureProgress > 0)
        {
            ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.g = this.captureProgress / 5;
            ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.r = 0;
            ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.a = this.interactTimer / 5;
        }
        else
        {
            if (this.captureProgress < 0)
            {
                ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.r = Mathf.Abs(this.captureProgress) / 5;
                ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.g = 0;
                ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.a = this.interactTimer / 5;
            }
            else
            {
                if (progressBar == 0)
                {
                    ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.r = 1;
                    ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.g = 1;
                    ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.b = 1;
                    ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.a = this.interactTimer / 5;
                }
                else
                {
                    ((SpriteRenderer) progressBar.GetComponent(typeof(SpriteRenderer))).color.b = 0;
                }
            }
        }
    }

    public virtual void checkInteractTimer() //Check time since last capture
    {
        if (this.interactTimer >= 0)
        {
            this.interactTimer = this.interactTimer - Time.deltaTime;
        }
        else
        {
            this.interactTimer = 0;
            this.captureProgress = 0;
        }
    }

    public virtual void CaptureLogic()
    {
        if (this.captureProgress >= this.captureTime)
        {
            this.spawnPlayerStation();
            UnityEngine.Object.Destroy(this.gameObject);
        }
        else
        {
            if (this.captureProgress <= -this.captureTime)
            {
                this.spawnEnemyStation();
                UnityEngine.Object.Destroy(this.gameObject);
            }
        }
    }

    public virtual void OnCollisionStay2D(Collision2D otherobj)//Check if any ships are touching me!
    {
        if ((otherobj.gameObject.tag == "Ship") || (otherobj.gameObject.tag == "SelectedShip"))//Player Capture!
        {
            this.captureProgress = this.captureProgress + Time.deltaTime;
            this.interactTimer = 5;
        }
        else
        {
            if (otherobj.gameObject.tag == "Enemy") //Enemy Capture!
            {
                this.captureProgress = this.captureProgress - Time.deltaTime;
                this.interactTimer = 5;
            }
        }
        if (otherobj.collider.tag == "Bullet")
        {
            this.captureProgress = this.captureProgress + Time.deltaTime;
            this.interactTimer = 5;
        }
        else
        {
            if (otherobj.collider.tag == "EnemyBullet")
            {
                this.captureProgress = this.captureProgress - Time.deltaTime;
                this.interactTimer = 5;
            }
        }
    }

    public EnemyStation()
    {
        this.captureTime = 5;
    }

}