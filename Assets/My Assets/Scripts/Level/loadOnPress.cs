using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class loadOnPress : MonoBehaviour
{
    public string sceneName;
    public virtual void Update()
    {
        if (Input.anyKeyDown)
        {
            Application.LoadLevel(this.sceneName);
        }
    }

    public loadOnPress()
    {
        this.sceneName = "menu";
    }

}