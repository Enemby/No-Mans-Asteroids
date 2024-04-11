using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class PlayerManager : MonoBehaviour
{
    //Manages all of our basic game variables and UI.
    public int minerals; //How much money do we got, homie?
    public GameObject player; //Who are we looking at, cuz?
    public Text mineralsUI;
    public GameObject controlsUI;
    public Text aiUI;
    public AIMode myAI;
    public Text squadUI;
    //var myOptions :GameObject;
    public virtual void Start()
    {
        if (PlayerPrefs.HasKey("Minerals") == false) //Yo, you got that written down dawg?
        {
            PlayerPrefs.SetInt("Minerals", 0);
            PlayerPrefs.Save(); //Ya boy, that's how we do.
        }
        else
        {
             //What it say tho
            this.minerals = PlayerPrefs.GetInt("Minerals"); //I hear you homie
        }
        this.setAIUI();
    }

    public virtual void setMineralUI()
    {
        this.mineralsUI.text = "Minerals: " + this.minerals;
    }

    public virtual void setAIUI() //Set our AI State UI.
    {
        string myText = "Fleet Orders: ";
        if (this.myAI == (AIMode) 0)
        {
            myText = myText + "Sentry";
        }
        else
        {
            if (this.myAI == (AIMode) 1)
            {
                myText = myText + "Follow";
            }
            else
            {
                if (this.myAI == (AIMode) 2)
                {
                    myText = myText + "Drift";
                }
            }
        }
        this.aiUI.text = myText;
    }

    public virtual void Update()
    {
        this.setMineralUI();
        if (Input.GetButtonDown("Menu") && (GameObject.FindGameObjectWithTag("PauseMenu") == null))
        {
            AsyncOperation @async = Application.LoadLevelAdditiveAsync("options");
            //yield async;
            Debug.Log("Loading complete");
            Time.timeScale = 0;
        }
        if (Input.GetButtonDown("AIMode"))
        {
            this.myAI = this.myAI + 1;
            if (this.myAI > (AIMode) 2)
            {
                this.myAI = (AIMode) 0;
            }
            this.setAIUI();
        }
        if (Input.GetButtonDown("Controls"))
        {
            if (this.controlsUI.active == true)
            {
                this.controlsUI.active = false;
                PlayerPrefs.SetInt("Controls", 0);
            }
            else
            {
                this.controlsUI.active = true;
                PlayerPrefs.SetInt("Controls", 1);
            }
            PlayerPrefs.Save();
        }
    }

}