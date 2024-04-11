using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.RequireComponent(typeof(AudioSource))]
public partial class MusicInstance : MonoBehaviour
{
    public AudioClip[] myClips;
    public string[] myNames;
    private AudioSource mySource;
    public int lastClipIndex;
    public bool fade;
    public bool updatedInfo;
    public int currentIndex;
    public bool skiptrack;
    public virtual void Start()
    {
        this.mySource = (AudioSource) this.GetComponent(typeof(AudioSource));
    }

    public virtual bool musicPlaying()
    {
        if (this.mySource.isPlaying == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void Deactivate()
    {
        this.fade = true;
    }

    public virtual void Activate()
    {
        this.fade = false;
    }

    public virtual AudioClip pickRandomClip()
    {
        object rand = null;
        if (this.myClips.Length <= 1)
        {
            rand = Random.Range(0, this.myClips.Length + 1);
        }
        else
        {
            rand = 0;
        }
        if (!(rand == this.lastClipIndex))
        {
            this.lastClipIndex = (int) rand;
            this.currentIndex = (int) rand;
            return this.myClips[rand];
        }
        else
        {
            rand = Random.Range(0, this.myClips.Length + 1);
            this.currentIndex = (int) rand;
            return this.myClips[rand];
        }
    }

    public virtual void updateMusicInfo()
    {
        GameObject myInfo = GameObject.FindGameObjectWithTag("MusicInfo");
        if (myInfo != null)
        {
            myInfo.GetComponent(UI.Text).text = this.myNames[this.currentIndex];
            myInfo.GetComponent("MusicInfo").BroadcastMessage("startPopUp");
        }
    }

    public virtual void Update()
    {
        if (Input.GetButtonDown("Skip"))
        {
            if (this.fade == false)
            {
                this.skiptrack = true;
            }
        }
        if (Application.isLoadingLevel)
        {
            Debug.Log("Loading level! Fading music");
            this.fade = true;
        }
    }

    public virtual void LateUpdate()
    {
        if ((this.musicPlaying() == false) || (this.skiptrack == true))
        {
            this.mySource.clip = this.pickRandomClip();
            this.mySource.Play();
            this.updatedInfo = false;
            this.skiptrack = false;
        }
        if (this.fade == true)
        {
            if (this.mySource.volume >= 0)
            {
                this.mySource.volume = this.mySource.volume - Time.deltaTime;
            }
        }
        else
        {
            if (this.updatedInfo == false)
            {
                this.updateMusicInfo();
                this.updatedInfo = true;
            }
            if (this.mySource.volume < PlayerPrefs.GetFloat("musicVolume"))
            {
                this.mySource.volume = this.mySource.volume + Time.deltaTime;
            }
            if (this.mySource.volume > PlayerPrefs.GetFloat("musicVolume"))
            {
                this.mySource.volume = this.mySource.volume - Time.deltaTime;
                if ((this.mySource.volume - PlayerPrefs.GetFloat("musicVolume")) <= 0.1f)
                {
                    this.mySource.volume = PlayerPrefs.GetFloat("musicVolume"); //This prevents jitter
                }
            }
        }
    }

}