using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public partial class FadeUIElement : MonoBehaviour
{
    public bool fade; //false fade out / true fade in
    public float fadevalue;
    public float targetAlpha;
    public float speed;
    public virtual void Update()
    {
        Color myCol = this.GetComponent<Graphic>().color;
        if ((this.fade == true) && (this.fadevalue < this.targetAlpha))
        {
            this.fadevalue = this.fadevalue + (Time.deltaTime * this.speed);
        }
        if ((this.fade == false) && (this.fadevalue > 0))
        {
            this.fadevalue = this.fadevalue - (Time.deltaTime * this.speed);
        }
        this.capFade();
        myCol.a = this.fadevalue;
        this.GetComponent<Graphic>().color = myCol;
    }

    public virtual void vignetteToggle(int mode)
    {
        if (mode == 1) //action
        {
            this.fade = true;
        }
        else
        {
            this.fade = false;
        }
    }

    public virtual void capFade()
    {
        if (this.fadevalue > this.targetAlpha)
        {
            this.fadevalue = this.targetAlpha;
        }
        if (this.fadevalue < 0)
        {
            this.fadevalue = 0;
        }
    }

    public FadeUIElement()
    {
        this.targetAlpha = 0.2f;
        this.speed = 0.5f;
    }

}