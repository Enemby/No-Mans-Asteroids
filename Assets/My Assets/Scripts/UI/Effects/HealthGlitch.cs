using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class HealthGlitch : MonoBehaviour
{
    //Check health percentage, do stuff.
    public MonoBehaviour noiseScript;
    public MonoBehaviour glitchScript;
    public object maxHealth;
    public object currentHealth;
    public float glitchTime;
    public float stayTime;
    public string shipTag;
    public bool multiplayer;
    private float timer;
    public virtual void UpdateReferences()
    {
        if (GameObject.FindGameObjectWithTag(this.shipTag) != null)
        {
            Component selectedShip = GameObject.FindGameObjectWithTag(this.shipTag).GetComponent("PlayerShip");
            if (this.multiplayer == true)
            {
                selectedShip = GameObject.FindGameObjectWithTag(this.shipTag).GetComponent("PlayerLocalMP");
            }
            this.maxHealth = selectedShip.maxHealth;
            this.currentHealth = selectedShip.shipHealth;
        }
    }

    public virtual void timerUpdate()
    {
        this.timer = this.timer + Time.deltaTime;
        if ((this.timer > this.glitchTime) && (this.timer < (this.glitchTime + this.stayTime)))
        {
            this.setGlitch(true);
        }
        else
        {
            this.setGlitch(false);
        }
        if (this.timer > (this.glitchTime + this.stayTime))
        {
            this.timer = 0;
        }
    }

    public virtual object calculatePercentage()
    {
        if (!(this.currentHealth == null) && !(this.maxHealth == null))
        {
            object percentage = this.currentHealth / this.maxHealth;
            percentage = ((int) percentage) * 100;
            return percentage;
        }
        else
        {
            return 100;
        }
    }

    public virtual void setGlitch(bool myMode)
    {
        if (myMode == true)
        {
            this.glitchScript.colorDrift = 0.5f;
            this.glitchScript.scanLineJitter = 0.3f;
        }
        else
        {
            this.glitchScript.colorDrift = 0;
            this.glitchScript.scanLineJitter = 0;
        }
    }

    public virtual void setNoiseIntensity()
    {
        if (this.calculatePercentage() == 100)
        {
            this.noiseScript.grainIntensityMax = 0;
            this.noiseScript.scratchIntensityMax = 0;
        }
        else
        {
             //This one problem, took me a full hour to figure out. (With help)
            this.noiseScript.grainIntensityMax = ((float) (100 / (int) this.calculatePercentage())) * 0.4f;//0.6 is full power
            this.noiseScript.scratchIntensityMax = 10 / (int) this.calculatePercentage();
        }
    }

    public virtual void EffectsCheck(bool myval)
    {
        if (myval == true)
        {
            this.noiseScript.enabled = true;
            this.glitchScript.enabled = true;
        }
        else
        {
            this.noiseScript.enabled = false;
            this.glitchScript.enabled = false;
        }
    }

    public virtual void Update()
    {
        this.UpdateReferences();
        if (this.calculatePercentage() <= 50)
        {
            this.timerUpdate();
        }
        else
        {
            this.setGlitch(false);
        }
        this.setNoiseIntensity();
        if (PlayerPrefs.GetInt("EffectsCheck") == 1)
        {
            this.EffectsCheck(true);
        }
        else
        {
            this.EffectsCheck(false);
        }
    }

    public HealthGlitch()
    {
        this.glitchTime = 1;
        this.stayTime = 0.2f;
        this.shipTag = "SelectedShip";
    }

}