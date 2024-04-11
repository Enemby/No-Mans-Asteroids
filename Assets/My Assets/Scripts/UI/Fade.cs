using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Fade : MonoBehaviour
{
    //Fades a SpriteRender alpha channel depending on vars set.
    public float fadeSpeed;
    public bool fadeIn;
    public bool loadScene;
    public string sceneName;
    public virtual void Update()
    {
        Color myCol = ((SpriteRenderer)this.GetComponent(typeof(SpriteRenderer))).color;
        if (this.fadeIn == false)
        {
            
            myCol.a = ((SpriteRenderer) this.GetComponent(typeof(SpriteRenderer))).color.a - this.fadeSpeed;
        }
        else
        {
            myCol.a = ((SpriteRenderer) this.GetComponent(typeof(SpriteRenderer))).color.a + this.fadeSpeed;
        }
        if ((myCol.a <= 0) || (((SpriteRenderer) this.GetComponent(typeof(SpriteRenderer))).color.a >= 1))
        {
            if (this.loadScene == true)
            {
                Application.LoadLevel(this.sceneName);
            }
            else
            {
               Destroy(this.gameObject, 50); //Make sure we clean up eventually...
            }
        }
        ((SpriteRenderer)this.GetComponent(typeof(SpriteRenderer))).color = myCol;
    }

    public Fade()
    {
        this.fadeSpeed = 0.1f;
        this.sceneName = "null";
    }

}