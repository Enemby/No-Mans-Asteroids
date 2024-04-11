using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MaxVolume : MonoBehaviour
{
    public virtual void Update()
    {
        float maxVolume = PlayerPrefs.GetFloat("maxVolume");
        AudioListener.volume = maxVolume;
    }

}