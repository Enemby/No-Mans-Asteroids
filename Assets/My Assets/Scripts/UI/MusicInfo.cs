using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public partial class MusicInfo : MonoBehaviour
{
    public Color ColorA;
    public Color ColorB;
    public float blendTime;
    public Text myText;
    public bool fade;
    public float timer;
    public virtual void startPopUp()
    {
        this.fade = true;
    }

    public virtual void UpdateText()
    {
        this.ColorA.a = this.timer;
        this.ColorB.a = this.timer;
        this.myText.color = Color.Lerp(this.ColorA, this.ColorB, Mathf.PingPong(Time.time * this.blendTime, 1));
        this.timer = this.timer + (Time.deltaTime * 0.5f);
        if (this.timer > 1)
        {
            this.timer = 1;
        }
        if (this.timer == 1)
        {
            this.fade = false;
        }
    }

    public virtual void endPopUp()
    {
        this.ColorA.a = this.timer;
        this.ColorB.a = this.timer;
        this.myText.color = Color.Lerp(this.ColorA, this.ColorB, Mathf.PingPong(Time.time * this.blendTime, 1));
        this.timer = this.timer - (Time.deltaTime * 0.5f);
        if (this.timer < 0)
        {
            this.timer = 0;
        }
        if (this.timer == 0)
        {
            this.fade = false;
        }
    }

    public virtual void Update()
    {
        if (this.fade == true)
        {
            this.UpdateText();
        }
        else
        {
            this.endPopUp();
        }
    }

    public MusicInfo()
    {
        this.timer = 1;
    }

}