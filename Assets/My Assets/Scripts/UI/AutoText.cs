using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public partial class AutoText : MonoBehaviour
{
    //AutoType.js that I found on the internet. I've modified it to have a lot more styling options, and be compatiable with Unity5 UI.
    [UnityEngine.Header("@/Red #/Green $/Gold %/Magenta ¬/Bold /Italics")]
    public float letterPause;
    public string word;
    public int progress;
    public bool fade;
    public AudioClip blipSound;
    private Text myText;
    private int textLength;
    public virtual void Start()
    {
        this.myText = GetComponent<Text>();
        this.word = GetComponent<Text>().text;
        this.textLength = (int) this.myText.text.Length;
        GetComponent<Text>().text = "";
        this.StartCoroutine(this.TypeText());
        this.InvokeRepeating("finishCheck", 10, 3);
    }

    public virtual void finishCheck() //Check if we're done.
    {
        if (this.progress >= (this.textLength - 1))//Time to fade!
        {
            if (this.progress != -100)
            {
                if (this.fade == true)
                {
                    this.InvokeRepeating("fadeText", 0, 0.025f);
                    this.progress = -100;
                }
            }
        }
    }

    public virtual void fadeText()
    {
        Color myCol = myText.color;
        myCol.a -= Time.deltaTime;
        myText.color = myCol;
        if (this.myText.color.a <= 0)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }

    public virtual IEnumerator TypeText()
    {
        bool bold = false; //toggles the style for bold;
        bool red = false; // toggle red
        bool green = false;
        bool gold = false;
        bool magenta = false;
        bool italics = false;
        bool ignore = false; //for ignoring special characters that toggle styles
        foreach (char nextletter in this.word.ToCharArray())
        {
            switch (nextletter)
            {
                case '@':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    red = !red; //toggle red styling
                    green = false; //toggle green styling
                    magenta = false; //This is so weird shit doesn't happen.
                    gold = false;
                    break;
                case '#':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    green = !green; //toggle green styling
                    magenta = false; //toggle green styling
                    red = false; //This is so weird shit doesn't happen.
                    gold = false;
                    break;
                case '$':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    green = false; //toggle green styling
                    red = false; //This is so weird shit doesn't happen.
                    magenta = false;
                    gold = !gold;
                    break;
                case '%':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    green = false; //toggle green styling
                    red = false; //This is so weird shit doesn't happen.
                    gold = false;
                    magenta = !magenta;
                    break;
                case '¬':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    bold = !bold; //toggle bold styling
                    break;
                case '/':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    italics = !italics; //toggle italic styling
                    break;
            }
            string letter = nextletter.ToString();
            if (!ignore)
            {
                if (bold)
                {
                    letter = ("<b>" + letter) + "</b>";
                }
                if (italics)
                {
                    letter = ("<i>" + letter) + "</i>";
                }
                if (red)
                {
                    letter = ("<color=#ff0000>" + letter) + "</color>";
                }
                else
                {
                    if (green)
                    {
                        letter = ("<color=#00ff00>" + letter) + "</color>";
                    }
                    else
                    {
                        if (gold)
                        {
                            letter = ("<color=#ffff00>" + letter) + "</color>";
                        }
                        else
                        {
                            if (magenta)
                            {
                                letter = ("<color=#cc00ff>" + letter) + "</color>";
                            }
                        }
                    }
                }
                this.myText.text += letter;
                this.progress++;
            }
            else
            {
                this.progress++; //Without this, we are off by one everytime styling is used.
            }
             //make sure the next character isn't ignored
            ignore = false;
            yield return new WaitForSeconds(this.letterPause);
            if (this.blipSound != null)
            {
                ((AudioSource) this.GetComponent(typeof(AudioSource))).clip = this.blipSound;
                ((AudioSource) this.GetComponent(typeof(AudioSource))).PlayOneShot(this.blipSound, 1);
            }
        }
    }

    public AutoText()
    {
        this.letterPause = 0.2f;
        this.fade = true;
    }

}