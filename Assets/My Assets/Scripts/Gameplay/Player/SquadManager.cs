using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SquadManager : MonoBehaviour
{
    public int selectedIndex;
    private bool shipSelected;
    private GameObject selectedShip; //Ship we have selected
    public bool squadSelected;
    public GameObject basicShip;
    public bool allowEmpty; //Should we let the squad be empty?
    public GameObject myCam;
    public virtual void Start()
    {
        this.InvokeRepeating("respawnCheck", 0f, 4f);
    }

    public virtual void Update()
    {
        if (Input.GetButtonDown("SwitchShip") && (this.squadSelected == true))
        {
            this.switchSelected();
        }
        if (Time.timeScale > 0)
        {
            if ((this.squadSelected == true) && (this.shipSelected == false)) //Enable a ship ONLY if the squad is selected.
            {
                this.checkSelection();
                this.setCameraTarget();
            }
            else
            {
                this.clearSelection();
            }
        }
    }

    public virtual void respawnCheck()
    {
        if ((this.transform.childCount == 0) && (this.allowEmpty == false))
        {
            this.respawnSquad();
        }
    }

    /*
			for(var i = 0;i < this.transform.childCount;i++){
				if(this.transform.GetChild(i).GetComponent("PlayerShip").selected == true){
					selectedIndex = i;
					shipSelected = true;
				}
			}
			*/    public virtual void checkSelection()
    {
        if (this.transform.childCount >= 1)
        {
            if (this.transform.GetChild(this.selectedIndex) != null)
            {
                if (this.transform.GetChild(this.selectedIndex).GetComponent("PlayerShip"))
                {
                    this.transform.GetChild(this.selectedIndex).GetComponent("PlayerShip").selected = true;
                }
                else
                {
                    this.transform.GetChild(this.selectedIndex).GetComponent("PlayerLocalMP").selected = true;
                }
                this.shipSelected = true;
            }
            else
            {
                this.selectedIndex = 0;
                this.transform.GetChild(0).GetComponent("PlayerShip").selected = true;
                this.shipSelected = true;
            }
        }
    }

    public virtual void clearSelection()
    {
        if (this.transform.childCount > 0)
        {
            this.shipSelected = false;
            int i = 0;
            while (i < this.transform.childCount)
            {
                if (this.transform.GetChild(this.selectedIndex).GetComponent("PlayerShip"))
                {
                    this.transform.GetChild(this.selectedIndex).GetComponent("PlayerShip").selected == false;
                }
                if (this.transform.GetChild(i).GetComponent("PlayerShip"))
                {
                    this.transform.GetChild(i).GetComponent("PlayerShip").selected == false;
                }
                i++;
            }
        }
    }

    public virtual void setCameraTarget()
    {
        if (this.transform.childCount > 0)
        {
            this.selectedShip = this.transform.GetChild(this.selectedIndex).gameObject;
            if (this.myCam == null)
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent("Follow Player").target = this.selectedShip;
            }
            else
            {
                this.myCam.GetComponent("Follow Player").target = this.selectedShip;
            }
        }
    }

    public virtual void switchSelected()
    {
        if (this.transform.childCount > 0)
        {
            this.transform.GetChild(this.selectedIndex).GetComponent("PlayerShip").selected = false;
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
            this.transform.GetChild(this.selectedIndex).GetComponent("PlayerShip").selected = true;
        }
    }

    public virtual void respawnSquad()
    {
        GameObject simpleShip = UnityEngine.Object.Instantiate(this.basicShip, this.transform.position, Quaternion.identity);
        simpleShip.transform.parent = this.transform;
    }

}