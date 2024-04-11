using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ControlsCheck : MonoBehaviour
{
    public virtual void Start()
    {
        if (PlayerPrefs.GetInt("Controls") == 0)
        {
            this.gameObject.active = false;
        }
    }

}