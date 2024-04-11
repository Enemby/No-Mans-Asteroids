using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class UpdateSlider : MonoBehaviour
{
    public string myPref;
    public UI.Slider mySlider;
    public virtual void Start()
    {
        if (this.mySlider == false)
        {
            this.mySlider = (error) this.GetComponent("UI.Slider");
        }
        if (PlayerPrefs.HasKey(this.myPref))
        {
            this.mySlider.value = (error) PlayerPrefs.GetFloat(this.myPref);
        }
    }

}