using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SquadText : MonoBehaviour
{
    public GameObject parentObj;
    public UI.Text myText;
    public int index;
    public virtual void Update()
    {
        if (this.parentObj != null)
        {
            this.index = (int) this.parentObj.GetComponent("switchSquad").selectedIndex;
        }
        this.updateText();
    }

    public virtual void updateText()
    {
        this.myText.text = "Squad: " + this.parentObj.transform.GetChild(this.index).gameObject.name;
    }

}