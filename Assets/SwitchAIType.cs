using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public partial class SwitchAIType : MonoBehaviour
{
    public int myValue;
    public string[] text;
    public Text myUI;
    public string myPref;
    public virtual void Start()
    {
        this.setText();
    }

    public virtual void switchUp()
    {
        this.myValue++;
        this.loopCheck();
    }

    public virtual void switchDown()
    {
        this.myValue--;
        this.loopCheck();
    }

    public virtual void loopCheck()
    {
        if (this.myValue > (this.text.Length - 1))
        {
            this.myValue = 0;
        }
        else
        {
            if (this.myValue < 0)
            {
                this.myValue = this.text.Length - 1;
            }
        }
    }

    public virtual void setText()
    {
        int output = 0;
        this.myUI.text = this.text[this.myValue];
        if (int.TryParse(this.text[this.myValue], out output))
        {
            int myInt = int.Parse(this.text[this.myValue]);
            PlayerPrefs.SetInt(this.myPref, myInt);
            PlayerPrefs.Save();
        }
    }

    public SwitchAIType()
    {
        this.myPref = "defaultPref";
    }

}