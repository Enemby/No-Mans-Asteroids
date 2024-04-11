using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class CutsceneHandler : MonoBehaviour
{
    public GameObject[] ourSteps;
    public int interval;
    public string nextScene;
    public GameObject loadingText;
    public float myTimer;
    public virtual void Update()
    {
        this.myTimer = this.myTimer + Time.deltaTime;
        if (this.myTimer >= 2)
        {
            if (Input.anyKeyDown)
            {
                this.iterateArray();
            }
        }
    }

    public virtual void Start()
    {
        if (PlayerPrefs.GetInt("EffectsCheck") == 0)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Kino.Tube>().enabled = false;
        }
    }

    public virtual void iterateArray()
    {
        if ((this.interval + 1) < this.ourSteps.Length)
        {
            this.interval = this.interval + 1;
        }
        else
        {
            GameObject myText = UnityEngine.Object.Instantiate(this.loadingText, this.transform.position, Quaternion.identity);
            myText.transform.SetParent(GameObject.Find("Canvas").transform, false);
            Application.LoadLevel(this.nextScene);
        }
        this.ourSteps[this.interval - 1].active = false;
        this.ourSteps[this.interval].active = true;
    }

}