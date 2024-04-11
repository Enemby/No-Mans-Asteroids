using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public partial class UITextTimer : MonoBehaviour
{
    public float timeToWait;
    public string myText;
    public virtual void Update()
    {
        if (Time.deltaTime != 0)
        {
            this.timeToWait = this.timeToWait - Time.deltaTime;
        }
        if (this.timeToWait <= 0)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
        int myInt = (int) this.timeToWait;
        this.GetComponent<Text>().text = (this.myText + myInt) + "s";
    }

    public UITextTimer()
    {
        this.timeToWait = 50;
    }

}