using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class E3 : MonoBehaviour
{
    //run this script on the "E3" gameobject. This will trigger the game ending.
    public GameObject endScreen;
    public AudioClip ourSound;
    private AudioSource mySource;
    public virtual void Start()
    {
        this.mySource = (AudioSource) this.GetComponent(typeof(AudioSource));
    }

    public virtual void OnTriggerEnter2D(Collider2D myobj)
    {
        if (myobj.transform.tag == "SelectedShip")
        {
            this.mySource.clip = this.ourSound;
            this.mySource.Play();
            GameObject endGO = UnityEngine.Object.Instantiate(this.endScreen, this.transform.position, Quaternion.identity);
        }
    }

}