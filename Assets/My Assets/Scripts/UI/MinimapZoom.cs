using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MinimapZoom : MonoBehaviour
{
    public GameObject cornerMap;
    public GameObject zoomMap;
    public virtual void Start()
    {
        this.getPreferences();
    }

    public virtual void toggleMaps()
    {
        if (this.cornerMap.active == true)
        {
            this.zoomMap.active = true;
            this.cornerMap.active = false;
        }
        else
        {
            this.zoomMap.active = false;
            this.cornerMap.active = true;
        }
        this.savePreferences();
    }

    public virtual void Update()
    {
        if (Input.GetButtonDown("MinimapZoom"))
        {
            this.toggleMaps();
        }
    }

    public virtual void savePreferences()
    {
        if (this.cornerMap.active == false)
        {
            PlayerPrefs.SetInt("Minimap", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Minimap", 0);
        }
        PlayerPrefs.Save();
    }

    public virtual void getPreferences()
    {
        int myPref = PlayerPrefs.GetInt("Minimap");
        if (myPref == 1)
        {
            this.zoomMap.active = true;
            this.cornerMap.active = false;
        }
        else
        {
            this.zoomMap.active = false;
            this.cornerMap.active = true;
        }
    }

}