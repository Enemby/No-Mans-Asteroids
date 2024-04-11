using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class titlescreen : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject creditsPanel;
    public GameObject instantActionPanel;
    public GameObject playPanel;
    public GameObject optionsPanel;
    public string levelToLoad;
    public string instantLevelToLoad;
    public GameObject loadingText;
    //Quick and dirty titlescreen.
    public virtual void Play()
    {
        this.menuPanel.active = false;
        this.playPanel.active = true;
    }

    public virtual void InstantActionSelect()
    {
        this.instantActionPanel.active = true;
        this.playPanel.active = false;
    }

    public virtual void showLoad()
    {
        this.loadingText.active = true;
    }

    /*
	else{
		Application.LoadLevel("beginnertutorial");
	}
	*/    public virtual void CampaignPlay()
    {
        if (PlayerPrefs.GetInt("tutorialCheck") == 1)
        {
            this.showLoad();
            Application.LoadLevel(this.levelToLoad);
        }
    }

    public virtual void Options()
    {
        this.optionsPanel.active = true;
        this.menuPanel.active = false;
    }

    public virtual void Credits()
    {
        this.creditsPanel.active = true;
        this.menuPanel.active = false;
    }

    public virtual void Back()
    {
        this.creditsPanel.active = false;
        this.optionsPanel.active = false;
        this.instantActionPanel.active = false;
        this.playPanel.active = false;
        this.menuPanel.active = true;
    }

    public virtual void Quit()
    {
        Application.Quit();
    }

    public virtual void AsteroidsUpdate(string mycount)
    {
        PlayerPrefs.SetInt("IA_ASTEROIDS", int.Parse(mycount));
        PlayerPrefs.Save();
    }

    public virtual void InstantActionStart()
    {
        this.loadingText.active = true;
        Application.LoadLevel(this.instantLevelToLoad);
    }

    public virtual void OldUpdate()
    {
        if (this.creditsPanel.active == true)
        {
            if (Input.anyKeyDown)
            {
                this.creditsPanel.active = false;
                this.menuPanel.active = true;
            }
        }
        else
        {
            if (Input.GetKeyDown("1"))
            {
                Application.LoadLevel("main");
            }
            else
            {
                if (Input.GetKeyDown("2"))
                {
                    this.creditsPanel.active = true;
                    this.menuPanel.active = false;
                }
                else
                {
                    if (Input.GetKeyDown("3"))
                    {
                        Application.Quit();
                    }
                }
            }
        }
    }

    public virtual void LoadLevel(string myLevel)
    {
        this.showLoad();
        Application.LoadLevel(myLevel);
    }

    public virtual void hideNewbieCheck()
    {
        GameObject myCheck = GameObject.FindGameObjectWithTag("NewbieCheck");
        myCheck.active = false;
        this.bypassNewbieCheck();
    }

    public virtual void Start()
    {
        this.setInit();
        this.AsteroidsUpdate("1000"); //This prevents 0 asteroids if the defaults are left alone
    }

    public virtual void bypassNewbieCheck()
    {
        PlayerPrefs.SetInt("tutorialCheck", 1);
        PlayerPrefs.Save();
    }

    public virtual void setInit() //check our initial prefs if not set set them.
    {
        if (!PlayerPrefs.HasKey("tutorialCheck"))
        {
            PlayerPrefs.SetInt("tutorialCheck", 0);
        }
        if (!PlayerPrefs.HasKey("maxVolume"))
        {
            PlayerPrefs.SetFloat("maxVolume", 1);
        }
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
        }
        if (!PlayerPrefs.HasKey("Controls"))
        {
            PlayerPrefs.SetInt("Controls", 1);
        }
        if (!PlayerPrefs.HasKey("RoundTimer"))
        {
            PlayerPrefs.SetInt("RoundTimer", 1);
        }
        PlayerPrefs.Save();
    }

    public titlescreen()
    {
        this.levelToLoad = "tutorial";
        this.instantLevelToLoad = "instantaction";
    }

}