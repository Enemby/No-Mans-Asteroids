using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Flashing Text : MonoBehaviour
{
    public Color ColorA;
    public Color ColorB;
    public float blendTime;
    public UI.Text myText;
    public virtual void Start()
    {
        if (this.myText == null)
        {
            this.myText = GetComponent(UI.Text);
        }
    }

    public virtual void Update()
    {
        this.myText.color = Color.Lerp(this.ColorA, this.ColorB, Mathf.PingPong(Time.time * this.blendTime, 1));
    }

    public Flashing Text()
    {
        this.blendTime = 0.5f;
    }

}