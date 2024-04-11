using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class UpdateToggle : MonoBehaviour
{
    public string myPref;
    public int truevalue;
    public UI.Toggle myToggle;
    public virtual void Start()
    {
        if (this.myToggle == false)
        {
            this.myToggle = (error) this.GetComponent("UI.Toggle");
        }
        if (PlayerPrefs.HasKey(this.myPref))
        {
            if (PlayerPrefs.GetInt(this.myPref) == this.truevalue)
            {
                this.myToggle.isOn = true;
            }
            else
            {
                this.myToggle.isOn = false;
            }
        }
    }

    public UpdateToggle()
    {
        this.truevalue = 1;
    }

}