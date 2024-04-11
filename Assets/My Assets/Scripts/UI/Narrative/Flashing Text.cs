using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class FlashingText : MonoBehaviour
{
    public Color ColorA;
    public Color ColorB;
    public float blendTime;
    public Text myText;
    public virtual void Start()
    {
        if (this.myText == null)
        {
            this.myText = GetComponent<Text>();
        }
    }

    public virtual void Update()
    {
        this.myText.color = Color.Lerp(this.ColorA, this.ColorB, Mathf.PingPong(Time.time * this.blendTime, 1));
    }

    public FlashingText()
    {
        this.blendTime = 0.5f;
    }

}