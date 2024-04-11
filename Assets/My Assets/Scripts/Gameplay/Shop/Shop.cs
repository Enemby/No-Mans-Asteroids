using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Shop : MonoBehaviour
{
    //Allows us to buy ships
    public GameObject[] ships;
    public int[] prices;
    public GameObject shopCanvas;
    public float activeDistance;
    public bool shopActive;
    public Vector3 spawnPosition;
    public AudioClip buySound;
    public virtual void checkIfNear()
    {
        GameObject ship = GameObject.FindGameObjectWithTag("SelectedShip");
        if (ship != null)
        {
            if (Vector3.Distance(this.transform.position, ship.transform.position) < this.activeDistance)
            {
                this.shopCanvas.active = true;
                this.shopActive = true;
            }
            else
            {
                this.shopCanvas.active = false;
                this.shopActive = false;
            }
        }
    }

    public virtual void shopInput()
    {
        int i = 1;
        while (i < this.ships.Length) //This is the laziest fix ever. Let me explain why we start at 1.
        {
            //It's so we don't have to include 0 as an input, which our array starts at.
            //However,our KeyDown won't work properly, since the game view shows no 0 variable.
            //So we just pretend 0 doesn't exist, and extend our array arbitrarily. Lazy.
            if (Input.GetKeyDown(i + ""))
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().minerals >= this.prices[i])
                {
                    GameObject myShip = UnityEngine.Object.Instantiate(this.ships[i], this.spawnPosition, Quaternion.identity);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().minerals = ((int) GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().minerals) - this.prices[i];
                    myShip.transform.parent = GameObject.FindGameObjectWithTag("Squad Manager").transform;
                    ((AudioSource) this.GetComponent(typeof(AudioSource))).PlayOneShot(this.buySound, 1);
                }
            }
            i++;
        }
    }

    public virtual void Update()
    {
        this.checkIfNear();
        if (this.shopActive == true)
        {
            this.shopInput();
        }
    }

    public Shop()
    {
        this.activeDistance = 50;
    }

}