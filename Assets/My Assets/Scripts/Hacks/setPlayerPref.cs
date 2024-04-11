using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class setPlayerPref : MonoBehaviour
{
    //Set target pref, on start
    public string targetPref;
    public int targetValue;
    public virtual void Start()
    {
        PlayerPrefs.SetInt(this.targetPref, this.targetValue);
        PlayerPrefs.Save();
    }

}