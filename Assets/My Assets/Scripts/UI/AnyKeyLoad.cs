using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public partial class AnyKeyLoad : MonoBehaviour
{
    public string scenetoload;
    public bool toggleAnimation;
    public float speed;
    private bool timerstate; //true positive, false negative
    private float timer;
    public virtual void Update()
    {
        if (this.toggleAnimation != false)
        {
            if (this.timerstate == true)
            {
                this.timer = this.timer + (Time.deltaTime * this.speed);
            }
            else
            {
                this.timer = this.timer - (Time.deltaTime * this.speed);
            }
            this.timer = Mathf.Clamp(this.timer, 0, 1);
            Color myCol = GetComponent<Text>().color;
            myCol.a = timer;
            GetComponent<Text>().color = myCol;
            if (this.timer == 1)
            {
                this.timerstate = !this.timerstate;
            }
            else
            {
                if (this.timer == 0)
                {
                    this.timerstate = !this.timerstate;
                }
            }
        }
        if (Input.anyKeyDown)
        {
            Application.LoadLevel(this.scenetoload);
        }
    }

    public AnyKeyLoad()
    {
        this.speed = 5;
        this.timerstate = true;
    }

}