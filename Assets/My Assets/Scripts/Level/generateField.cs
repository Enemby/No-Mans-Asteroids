using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class generateField : MonoBehaviour
{
    public int asteroids;
    public float maxScale;
    public int spawnRange;
    public GameObject myAsteroid;
    public virtual void Start()
    {
        Random.seed = (int) System.DateTime.Now.Ticks;
        this.generateField();
    }

    public virtual void generateField() //Goal: generate an asteroid field.
    {
        int i = 0;
        while (i < this.asteroids)
        {
            GameObject newAsteroid = UnityEngine.Object.Instantiate(this.myAsteroid, this.transform.position, Quaternion.identity);
            newAsteroid.transform.position = new Vector3(Random.Range(-this.spawnRange, this.spawnRange), Random.Range(-this.spawnRange, this.spawnRange), 0);
            float myScale = Random.Range(0.26f, this.maxScale);
            newAsteroid.transform.localScale.x = myScale;
            newAsteroid.transform.localScale.y = myScale;
            i++;
        }
    }

    public generateField()
    {
        this.asteroids = 200;
        this.maxScale = 3;
        this.spawnRange = 10000;
    }

}