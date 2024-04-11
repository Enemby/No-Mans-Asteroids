using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class MainMenuRip : MonoBehaviour
{
    public string link;
    public WWW web;
    public virtual void Start()
    {
        this.web = new WWW(this.link);
    }

    public virtual void Update()
    {
        if (this.web.isDone)
        {
            error mytext = this.GetComponent(UI.Text);
            mytext.text = this.web.text;
        }
    }

    public MainMenuRip()
    {
        this.link = "https://docs.google.com/document/u/0/export?format=txt&id=1nov4VLgqCvkd-Pf-jETXeV26And5FifPW6J5KvWj2v4&token=AC4w5Vi3SiKTrGfWIlqizygaKRgg5IxtPQ%3A1502060542977&includes_info_params=true";
    }

}