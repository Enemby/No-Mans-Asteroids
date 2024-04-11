using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class NeutalStation : MonoBehaviour
{
    public float captureProgress;
    public float captureTime;
    public GameObject playerStation;
    public GameObject neutralStation;
    public GameObject enemyStation;
    public float interactTimer; //Decrease capture after certain time
    //This script creates a neutral space station, that can be captured by either team.
    public virtual void Update()
    {
        this.checkInteractTimer();
        this.CaptureLogic();
        this.setProgressUI();
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
        SpriteRenderer myRend = progressBar.GetComponent<SpriteRenderer>();
        Color colorBar = myRend.color;
        if (this.captureProgress > 0)
        {
            colorBar.g = this.captureProgress / 5;
            colorBar.r = 0;
            colorBar.a = this.interactTimer / 5;
        }
        else
        {
            if (this.captureProgress < 0)
            {
                colorBar.r = Mathf.Abs(this.captureProgress) / 5;
                colorBar.g = 0;
                colorBar.a = this.interactTimer / 5;
            }
            else
            {
                if (progressBar == null)
                { //TODO: Verify this works properly
                    colorBar.r = 1;
                    colorBar.g = 1;
                    colorBar.b = 1;
                    colorBar.a = this.interactTimer / 5;
                }
                else
                {
                    colorBar.b = 0;
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
    }

    public NeutalStation()
    {
        this.captureTime = 5;
    }

}