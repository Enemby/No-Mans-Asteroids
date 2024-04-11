using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Options : MonoBehaviour
{
    public string mypref;
    public GameObject canvasObject;
    public GameObject pauseMenu;
    public virtual void setPrefName(string prefset)
    {
        this.mypref = prefset;
    }

    public virtual void setFloatPref(float myvalue)
    {
        if ((this.mypref != "") && (this.mypref != null))
        {
            PlayerPrefs.SetFloat(this.mypref, myvalue);
            PlayerPrefs.Save();
        }
    }

    public virtual void setIntPref(int myvalue)
    {
        if ((this.mypref != "") && (this.mypref != null))
        {
            PlayerPrefs.SetInt(this.mypref, myvalue);
            PlayerPrefs.Save();
        }
    }

    public virtual void ToggleIntPref(string prefset)
    {
        if (PlayerPrefs.GetInt(prefset) == 1)
        {
            PlayerPrefs.SetInt(prefset, 0);
        }
        else
        {
            PlayerPrefs.SetInt(prefset, 1);
        }
        PlayerPrefs.Save();
    }

    public virtual void closeMenu()
    {
        this.canvasObject.active = false;
        this.pauseMenu.active = true;
    }

    public virtual void openMenu()
    {
        this.canvasObject.active = true;
        this.pauseMenu.active = false;
    }

    public virtual void deleteMenu()
    {
        Time.timeScale = 1;
        UnityEngine.Object.Destroy(this.canvasObject.gameObject.transform.root.gameObject);
        UnityEngine.Object.Destroy(this.gameObject);
    }

    public virtual void quitToTitle()
    {
        if (!(Application.loadedLevel == "menu"))
        {
            Application.LoadLevel("menu");
        }
        else
        {
            Application.Quit();
        }
    }

}