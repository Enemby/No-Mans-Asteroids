using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public partial class UpdateToggle : MonoBehaviour
{
    public string myPref;
    public int truevalue;
    public Toggle myToggle;
    public virtual void Start()
    {
        if (this.myToggle == false)
        {
            this.myToggle = this.GetComponent<Toggle>();
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