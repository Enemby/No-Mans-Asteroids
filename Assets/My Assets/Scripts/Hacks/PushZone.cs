using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PushZone : MonoBehaviour
{
    public string myTag;
    public bool checkChildren; //Check if theres an object we could be effecting
    public GameObject checkChildrenObj;
    public bool alreadyTriggered;
    public Vector2 pushSpeed;
    private float timer;
    private float timerMax;
    public virtual void Update() //Hi Cat!
    {
        this.timer = this.timer + Time.deltaTime;
        if (this.timer >= this.timerMax)
        {
            this.timer = 0;
            ((Collider2D) this.GetComponent(typeof(Collider2D))).enabled = false;
            ((Collider2D) this.GetComponent(typeof(Collider2D))).enabled = true;
        }
        this.checkTriggers();
        if (this.checkChildren == true)
        {
            if (this.checkChildrenObj.transform.GetChildCount() > 0)
            {
                ((Collider2D) this.GetComponent(typeof(Collider2D))).enabled = true;
            }
            else
            {
                ((Collider2D) this.GetComponent(typeof(Collider2D))).enabled = false;
            }
        }
    }

    public virtual void checkTriggers()
    {
        if (this.alreadyTriggered == false)
        {
            if (this.checkChildren == true)
            {
                if (this.checkChildrenObj.transform.GetChildCount() == 0)
                {
                    this.alreadyTriggered = false;
                }
            }
        }
    }

    public virtual void pushTarget(GameObject myObj)
    {
        if ((Rigidbody2D) myObj.GetComponent(typeof(Rigidbody2D)))
        {
            ((Rigidbody2D) myObj.GetComponent(typeof(Rigidbody2D))).AddForce(this.pushSpeed);
        }
    }

    /*
function OnTriggerEnter2D(otherobj : Collider2D){
		if(otherobj.gameObject.tag == myTag){
		}
}
*/
    public virtual void OnTriggerStay2D(Collider2D otherobj)
    {
        if (otherobj.gameObject.tag == this.myTag)
        {
            if (this.alreadyTriggered == false)
            {
                this.pushTarget(otherobj.gameObject);
            }
        }
    }

    public virtual void OnTriggerExit2D(Collider2D otherobj)
    {
        if (otherobj.gameObject.tag == this.myTag)
        {
            ((Collider2D) this.GetComponent(typeof(Collider2D))).enabled = false;
            this.alreadyTriggered = true;
        }
    }

    public PushZone()
    {
        this.myTag = "SelectedShip";
        this.timerMax = 1.5f;
    }

}