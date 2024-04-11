using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public partial class UpdateSlider : MonoBehaviour
{
    public string myPref;
    public Slider mySlider;
    public virtual void Start()
    {
        if (this.mySlider == false)
        {
            this.mySlider = this.GetComponent<Slider>();
        }
        if (PlayerPrefs.HasKey(this.myPref))
        {
            this.mySlider.value = PlayerPrefs.GetFloat(this.myPref);
        }
    }

}