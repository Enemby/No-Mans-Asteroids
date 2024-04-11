using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class CheckTutorialCondition : MonoBehaviour
{
    public string checkTag;
    public GameObject toggledObject;
    [UnityEngine.Header("If false, triggers on finding no commander")]
    public bool checkExists;
    public virtual void FixedUpdate()
    {
        if (this.checkExists == true)
        {
            if (GameObject.FindGameObjectWithTag(this.checkTag) != null)
            {
                this.toggledObject.active = true;
                this.gameObject.active = false;
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag(this.checkTag) == null)
            {
                this.toggledObject.active = true;
                this.gameObject.active = false;
            }
        }
    }

    public CheckTutorialCondition()
    {
        this.checkExists = true;
    }

}