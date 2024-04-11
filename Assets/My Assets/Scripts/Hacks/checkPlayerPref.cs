using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class checkPlayerPref : MonoBehaviour
{
    //Check if playerpref is equal to value.
    //If so, enable object.
    public string myPref; //Could use support for any kind of pref.
    public int requestedValue;
    public GameObject targetObject;
    public virtual void Start()//do nothing
    {
        if (PlayerPrefs.HasKey(this.myPref))
        {
            if (PlayerPrefs.GetInt(this.myPref) == this.requestedValue)
            {
                this.targetObject.active = true;
            }
        }
        else
        {
        }
    }

    public virtual void Update()
    {
        if (PlayerPrefs.GetInt(this.myPref) != this.requestedValue)
        {
            this.targetObject.active = false;
        }
    }

}