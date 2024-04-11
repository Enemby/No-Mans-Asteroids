using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class InstantActionLoad : MonoBehaviour
{
    public GameObject neutralStation;
    public GameObject player;
    public GameObject enemyShip;
    public float spawnDistance;
    public bool localMP;
    private GameObject asteroidGen;
    private GameObject eCommander;
    private object stationCount;
    public virtual void setReferences() //Update our references (this saves frames)
    {
        this.asteroidGen = GameObject.FindGameObjectWithTag("AsteroidGen");
        this.eCommander = GameObject.FindGameObjectWithTag("EAICOM");
    }

    public virtual void setSettings() //Set Instant Action Preferences from the menu
    {
        this.asteroidGen.GetComponent<generateFields>().asteroids = PlayerPrefs.GetInt("IA_ASTEROIDS");
        this.asteroidGen.GetComponent<generateFields>().spawnRange = PlayerPrefs.GetInt("IA_DENSITY");
        if (this.eCommander)
        {
            this.eCommander.GetComponent<AICommander>().myType = (CommanderType)PlayerPrefs.GetInt("IA_AITYPE");
        }
        this.stationCount = PlayerPrefs.GetInt("IA_STATIONS");
    }

    public virtual void Awake()//On scene load, do this:
    {
        this.setReferences();
        this.setSettings();
        if (this.localMP == false)
        {
            this.spawnMap();
        }
    }

    public virtual void spawnMap()
    {
        int i = 0;
        while (i < (((int) this.stationCount) - 1))
        {
            if (i == 0)
            {
                this.player.transform.position = new Vector3(-20, 0, 0);
            }
            UnityEngine.Object.Instantiate(this.neutralStation, new Vector3(i * this.spawnDistance, 0, 0), Quaternion.identity);
            if (i >= (((int) this.stationCount) - 2))
            {
                this.enemyShip.transform.position = new Vector3((i * this.spawnDistance) + 10, 0, 0);
            }
            i++;
        }
    }

    public InstantActionLoad()
    {
        this.spawnDistance = 400;
    }

}