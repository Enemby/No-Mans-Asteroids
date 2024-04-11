using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class PlayerLocalMP : MonoBehaviour
{
    //Controls our ships
    public ShipType myType; //0/Fighter | 1/Harvester
    public bool selected;
    public bool allowInput;
    public GameObject bullet; //gameobject
    public Rigidbody2D myRigidbody;
    public AudioClip fireSound;
    public AudioClip explosionSound;
    public AudioClip damagedSound;
    public AudioClip slowSound;
    public float speed; //How quickly do we accelerate?
    public float turnSpeed; //How fast can we rotate and change orientation?
    public float cooldown; //How long until we can fire our weapon?
    public float maxHealth; //How much health can we have if fully healed?
    public float maxSpeed; //How fast can we move?
    public float fireForce; //How fast does our bullet move?
    public GameObject explosionParticle; //Explosion effect when we die :(
    private float fireTimer; //Cooldown timer. You shouldn't have to screw with this unless something goes terribly wrong!
    public float shipHealth; //current Health. This should stay private, to help stop weird health modification bugs
    public GameObject target;
    public virtual void Start()
    {
        this.myRigidbody = (Rigidbody2D) this.GetComponent(typeof(Rigidbody2D));
        this.shipHealth = this.maxHealth;
    }

    public virtual void maintainSpeed() //Slow down when above speed parameters
    {
        if (this.selected == true)
        {
            if (this.myRigidbody.velocity.magnitude >= this.maxSpeed)
            {
                this.myRigidbody.drag = 20;
            }
            else
            {
                this.myRigidbody.drag = 0.1f;
            }
        }
        else
        {
            if (this.myRigidbody.velocity.magnitude >= (this.maxSpeed * 0.5f)) //Bottleneck speed when we aren't controlling it!
            {
                this.myRigidbody.drag = 20;
            }
            else
            {
                this.myRigidbody.drag = 0.1f;
            }
        }
    }

    public virtual void Fire() //Shoot a bullet, or catch an Asteroid in a Harvester. No Caching! :(
    {
        if (this.fireTimer <= 0.01f)
        {
            this.fireTimer = this.cooldown;
            if (this.myType == (ShipType) 0) //Fighter
            {
                GameObject myBullet = UnityEngine.Object.Instantiate(this.bullet, this.transform.position, Quaternion.identity);
                Physics2D.IgnoreCollision((BoxCollider2D) myBullet.GetComponent(typeof(BoxCollider2D)), (BoxCollider2D) this.GetComponent(typeof(BoxCollider2D)));
                ((Rigidbody2D) myBullet.GetComponent(typeof(Rigidbody2D))).velocity = this.transform.up * this.fireForce;
                ((Rigidbody2D) myBullet.GetComponent(typeof(Rigidbody2D))).velocity = ((Rigidbody2D) myBullet.GetComponent(typeof(Rigidbody2D))).velocity + ((Rigidbody2D) this.GetComponent(typeof(Rigidbody2D))).velocity; //0.5 is more correct, but makes you feel like you shot at an angle at high speeds.
                ((AudioSource) this.GetComponent(typeof(AudioSource))).PlayOneShot(this.fireSound, 0.5f);
            }
        }
    }

    public virtual void shipInput() //Check for button presses, act accordingly.
    {
        //Movement
        if (this.allowInput == true)
        {
            this.transform.localRotation.eulerAngles.z = this.transform.localRotation.eulerAngles.z - (((Input.GetAxisRaw("HorizontalP2") * this.turnSpeed) * Time.deltaTime) * 14);
            this.myRigidbody.AddForce((this.transform.up * Input.GetAxisRaw("VerticalP2")) * this.speed); //2D physics
            if (Input.GetButton("SlowP2"))
            {
                this.myRigidbody.velocity = this.myRigidbody.velocity * 0.9f;
                if (this.myRigidbody.velocity.magnitude >= 10)
                {
                    ((AudioSource) this.GetComponent(typeof(AudioSource))).clip = this.slowSound;
                    if (((AudioSource) this.GetComponent(typeof(AudioSource))).isPlaying == false)
                    {
                        ((AudioSource) this.GetComponent(typeof(AudioSource))).Play();
                        ((AudioSource) this.GetComponent(typeof(AudioSource))).loop = true;
                    }
                }
            }
            if (Input.GetButton("FireP2"))
            {
                this.Fire();
            }
        }
    }

    public virtual void weaponTimer() //Iterate through the shooting cooldown.
    {
        if (this.fireTimer > 0)
        {
            this.fireTimer = this.fireTimer - Time.deltaTime;
        }
        if (this.fireTimer < 0)
        {
            this.fireTimer = 0;
        }
    }

    public virtual void Die() //Spawn an explosion, and delete the GameObject.
    {
        GameObject explosion = UnityEngine.Object.Instantiate(this.explosionParticle, this.transform.position, Quaternion.identity);
        GameObject mySound = new GameObject(); //Spawn explosion audio GameObject. Could've sworn there was a better way for this.
        mySound.transform.position = this.transform.position;
        mySound.AddComponent(typeof(AudioSource));
        ((AudioSource) mySound.GetComponent(typeof(AudioSource))).spatialBlend = 1;
        ((AudioSource) mySound.GetComponent(typeof(AudioSource))).PlayOneShot(this.explosionSound, 1);
        UnityEngine.Object.Destroy(mySound, 10); //Cleanup!
        UnityEngine.Object.Destroy(explosion, 20);
        UnityEngine.Object.Destroy(this.gameObject); //This should be changed, probably.
    }

    public virtual void OnCollisionEnter2D(Collision2D mycol) //Check if we're being hurt/shot at
    {
        if (mycol.collider.tag == "SpikeEnemy")
        {
            this.shipHealth = this.shipHealth - 1;
        }
        if (mycol.collider.tag == "Bullet")
        {
            this.shipHealth = this.shipHealth - 0.5f;
            if (this.selected == true)
            {
                GameObject.Find("Main Camera2").BroadcastMessage("ScreenShake");
                GameObject mySound = new GameObject(); //Spawn explosion audio GameObject. Could've sworn there was a better way for this.
                mySound.transform.position = this.transform.position;
                mySound.AddComponent(typeof(AudioSource));
                ((AudioSource) mySound.GetComponent(typeof(AudioSource))).spatialBlend = 0;
                ((AudioSource) mySound.GetComponent(typeof(AudioSource))).pitch = ((AudioSource) mySound.GetComponent(typeof(AudioSource))).pitch + (Random.Range(0, 1) * 0.1f);
                ((AudioSource) mySound.GetComponent(typeof(AudioSource))).PlayOneShot(this.damagedSound, 1);
                UnityEngine.Object.Destroy(mySound, 10); //Cleanup!
            }
            UnityEngine.Object.Destroy(mycol.gameObject);
        }
    }

    public virtual void clearSelection()
    {
        this.selected = false;
    }

    public virtual void Update() //Called every frame
    {
        if (this.shipHealth <= 0)
        {
            this.Die();
        }
        if (this.selected == true) //Are we controlling this ship?
        {
            this.myRigidbody.isKinematic = false;
            this.shipInput();
            this.transform.tag = "PlayerLocalMP";
        }
        this.weaponTimer();
    }

    public virtual float getMaxHealth()
    {
        return this.maxHealth;
    }

    public virtual float getShipHealth()
    {
        return this.shipHealth;
    }

    public PlayerLocalMP()
    {
        this.allowInput = true;
        this.speed = 0.1f;
        this.turnSpeed = 1.5f;
        this.cooldown = 0.5f;
        this.maxHealth = 0.1f;
        this.maxSpeed = 250;
        this.fireForce = 100;
        this.shipHealth = this.maxHealth;
    }

}