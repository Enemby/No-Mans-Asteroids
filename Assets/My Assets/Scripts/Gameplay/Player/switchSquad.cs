using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class switchSquad : MonoBehaviour
{
    public int selectedIndex;
    private bool squadSelected;
    public virtual void checkSelection()
    {
        if (this.transform.childCount > 0)
        {
            this.squadSelected = false;
            int i = 0;
            while (i < this.transform.childCount)
            {
                if (this.transform.GetChild(i).GetComponent<SquadManager>().squadSelected == true)
                {
                    this.selectedIndex = i;
                    this.squadSelected = true;
                }
                i++;
            }
            if (this.squadSelected == false) //Nothing is selected! Let's pick something! :D
            {
                this.selectedIndex = 0;
                this.transform.GetChild(0).GetComponent<SquadManager>().squadSelected = true;
            }
        }
    }

    public virtual void switchSelected()
    {
        if (this.transform.childCount > 0)
        {
            this.transform.GetChild(this.selectedIndex).GetComponent<SquadManager>().squadSelected = false;
            this.transform.GetChild(this.selectedIndex).GetComponent<SquadManager>().BroadcastMessage("clearSelection");
            if (this.selectedIndex == (this.transform.childCount - 1))
            {
                this.selectedIndex = 0;
            }
            else
            {
                if ((this.selectedIndex + 1) <= (this.transform.childCount - 1))
                {
                    this.selectedIndex = this.selectedIndex + 1;
                }
            }
            this.transform.GetChild(this.selectedIndex).GetComponent<SquadManager>().squadSelected = true;
        }
    }

    public virtual void checkEmpty()
    {
        if (this.transform.GetChild(this.selectedIndex).transform.childCount <= 0)
        {
            this.switchSelected();
        }
    }

    public virtual void Update()
    {
        if (Input.GetButtonDown("SwitchSquad") && (this.squadSelected == true))
        {
            this.switchSelected();
            this.checkEmpty();
            this.checkEmpty();
        }
        if (Time.deltaTime > 0)
        {
            this.checkSelection();
        }
    }

}