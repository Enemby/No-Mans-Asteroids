using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class LoadLevel : MonoBehaviour
{
    public string levelToLoad;
    public GameObject loadingText;
    public GameObject winningText;
    public virtual void Start()
    {
        if (this.winningText != null)
        {
            GameObject myText1 = UnityEngine.Object.Instantiate(this.winningText, this.transform.position, Quaternion.identity);
            myText1.transform.SetParent(GameObject.Find("Canvas").transform, false);
            if (PlayerPrefs.HasKey("RoundTimer") && (PlayerPrefs.GetInt("RoundTimer") == 1))
            {
                myText1.transform.GetChild(0).GetComponent("Text").text = ((string) myText1.transform.GetChild(0).GetComponent("Text").text) + (Mathf.RoundToInt(Time.timeSinceLevelLoad) + "s");
            }
            else
            {
                myText1.transform.GetChild(0).GetComponent("Text").text = ((string) myText1.transform.GetChild(0).GetComponent("Text").text) + (Time.timeSinceLevelLoad + "s");
            }
        }
    }

    public virtual void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            this.Load();
        }
    }

    public virtual void Load()
    {
        GameObject myText = UnityEngine.Object.Instantiate(this.loadingText, this.transform.position, Quaternion.identity);
        myText.transform.SetParent(GameObject.Find("Canvas").transform, false);
        Application.LoadLevel(this.levelToLoad);
    }

}