using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class CheckTutorialSquad : MonoBehaviour
{
    public string checkTag;
    public int index;
    public int targetSize;
    public GameObject toggledObject;
    public virtual void FixedUpdate()
    {
        if (GameObject.FindGameObjectWithTag(this.checkTag).transform.GetChild(this.index).childCount >= this.targetSize)
        {
            this.toggledObject.active = true;
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }

    public CheckTutorialSquad()
    {
        this.targetSize = 2;
    }

}