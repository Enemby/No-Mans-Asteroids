using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class AnimationZone : MonoBehaviour
{
    //TODO: Add check to stop animation zones activating without a respawn.
    public string myTag;
    public bool checkChildren; //Check if theres an object we could be effecting
    public GameObject checkChildrenObj;
    public bool alreadyTriggered;
    private float timer;
    private float timerMax;
    private int childCount;
    public virtual void Update()
    {
        this.checkTriggers();
        this.timer = this.timer + Time.deltaTime;
        if (this.timer >= this.timerMax)
        {
            this.timer = 0;
            ((Collider2D) this.GetComponent(typeof(Collider2D))).enabled = false;
            ((Collider2D) this.GetComponent(typeof(Collider2D))).enabled = true;
        }
    }

    public virtual void checkTriggers()
    {
        if (this.checkChildren == true)
        {
            if (this.checkChildrenObj.transform.GetChildCount() <= 0)
            {
                //alreadyTriggered = false;
                Debug.Log("Triggered: " + this.alreadyTriggered);
            }
        }
    }

    public virtual void RemoveControl(bool mybool, GameObject targetObject) //Stop ship from ruining animation
    {
        //Toggle AllowInput
        if (mybool == true)
        {
            targetObject.GetComponent("PlayerShip").allowInput = true;
        }
        else
        {
            //alreadyTriggered = true;
            targetObject.GetComponent("PlayerShip").allowInput = false;
        }
    }

    public virtual void toggleVisuals(bool mybool)
    {
        if (mybool == true)
        {
            ((SpriteRenderer) this.GetComponent(typeof(SpriteRenderer))).enabled = true;
        }
        else
        {
            ((SpriteRenderer) this.GetComponent(typeof(SpriteRenderer))).enabled = false;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D otherobj)
    {
        if (this.checkChildren == true)
        {
            if (this.checkChildrenObj.transform.GetChildCount() > 0)
            {
                if (otherobj.gameObject.tag == this.myTag)
                {
                    if (this.alreadyTriggered == false)
                    {
                        this.toggleVisuals(true);
                    }
                }
            }
        }
        else
        {
            if (otherobj.gameObject.tag == this.myTag)
            {
                this.toggleVisuals(true);
            }
        }
    }

    public virtual void OnTriggerStay2D(Collider2D otherobj)
    {
        if (otherobj.gameObject.tag == this.myTag)
        {
            if (this.alreadyTriggered == false)
            {
                this.RemoveControl(false, otherobj.gameObject);
            }
        }
    }

    public virtual void OnTriggerExit2D(Collider2D otherobj)
    {
        if (otherobj.gameObject.tag == this.myTag)
        {
            this.toggleVisuals(false);
            this.RemoveControl(true, otherobj.gameObject);
            this.alreadyTriggered = true;
        }
    }

    public AnimationZone()
    {
        this.myTag = "SelectedShip";
        this.timerMax = 1.5f;
    }

}