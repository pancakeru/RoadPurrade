using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    //movement and van
    private float moveSpeed = 0.05f;
    private Rigidbody2D rb;
    public bool jump = false;
    public bool grounded = true;
    public GameObject van;
    public vanScript vanControl;
    public bool driving;
    public bool carrying;
    private bool isWalking = false;
    private bool isWalking2 = false;
    public int resetTimer = 10;
    private int jumpDelay = 1;
    
    //van collider management
    private GameObject storageTrigger;
    private GameObject[] vanFloors;
    public int drive_delay;
    private Transform STPos;
    private Collider2D STCollider;
    public bool cantPush = false;

    //player status variables

    //hunger
    public int hungerLevel = 0;
    public int hungerTimer = 700;
    public bool eaten = false;
    public bool needFood = false;
    public bool cooking = false;
    public bool eating = false;

    //sleep
    public int tiredLevel = 0;
    public int tiredTimer = 2200;
    public bool slept = false;
    public bool needSleep = false;
    public bool fatigued = false;
    public bool sleeping = false;
   // private int sleepTimer = 10;
   // private bool set = false;
    public Sprite idleLeft;

    //hygiene
    public int stinkLevel = 0;
    public int stinkTimer = 1600;
    public bool showered = false;
    public bool needShower = false;
    public bool stinky = false;
    int poopTimer = 200;
    public GameObject poop;
    public bool cleaning;
    public bool showering = false;
    private GameObject flag;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        vanControl = van.GetComponent<vanScript>();
        driving = false;
        carrying = false;
        drive_delay = 10;

        storageTrigger = GameObject.FindWithTag("storage trigger");
        STPos = storageTrigger.GetComponent<Transform>();
        STCollider = storageTrigger.GetComponent<Collider2D>();

        vanFloors = GameObject.FindGameObjectsWithTag("van floor");
        flag = GameObject.FindWithTag("flag pic");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (MGMScript.gameInProgress == false)
        //{
        resetTimer--;
        jumpDelay--;

        if(!driving && resetTimer<= 0)
        {
            drive_delay = 10;
        }
        // sleepTimer--;

        // Debug.Log(fatigued);
        if (!showering && !sleeping && !cooking && !eating)
        {
            hungerTimer -= 1;
            tiredTimer -= 1;
            stinkTimer -= 1;
        }

        if (hungerTimer <= 0)
        {
            hungerLevel += 1;
            hungerTimer = 850;
        }

        if (hungerLevel >= 5)
        {
            needFood = true;
        }

        if(hungerLevel >= 7)
        {
            moveSpeed = 0.025f;
        } else
        {
            moveSpeed = 0.05f;
        }


        if (eaten)
        {
            hungerLevel = 0;
            hungerTimer = 850;
            eaten = false;
            needFood = false;
        }

        //tiredness stuff
        if (tiredTimer <= 0)
        {
            tiredLevel += 1;
            tiredTimer = 2000;
        }

        if (tiredLevel >= 5)
        {
            needSleep = true;
        }

        if (tiredLevel >= 7)
        {
            fatigued = true;
        }
        else
        {
            fatigued = false;
        }

        if (slept)
        {
            tiredLevel = 0;
            tiredTimer = 2000;
            slept = false;
            needSleep = false;
        }

       

        //hygiene stuff
        if (stinkTimer <= 0)
        {
            stinkLevel += 1;
            stinkTimer = 1400;
        }

        if (stinkLevel >= 5)
        {
            needShower = true;
        }

        if (stinkLevel >= 6)
        {
            stinky = true;
        }
        else
        {
            stinky = false;
        }

        if (stinky && !showering)
            Pooper();

        if (showered)
        {
            stinkLevel = 0;
            stinkTimer = 1400;
            showered = false;
            needShower = false;
        }

        if (showering || cooking || sleeping || eating)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
        }

        //player movement
        if (Input.GetKey(KeyCode.D) && !cantPush && !showering && !sleeping && !cooking && !eating)
        {
             this.transform.position += new Vector3(moveSpeed, 0, 0);
            // rb.AddForce(Vector2.right * moveSpeed);
            isWalking = true;

        }
        else
        {
            isWalking = false;
        }

        if (Input.GetKey(KeyCode.A) && !showering && !sleeping && !cooking && !eating)
        {
            this.transform.position += new Vector3(-moveSpeed, 0, 0);
            //rb.AddForce(Vector2.left * moveSpeed);
            isWalking2 = true;
       

        } else
        {
            isWalking2 = false;
        }

        

        if (Input.GetKey(KeyCode.W) && grounded && !driving && !showering && !sleeping && !cooking && !eating)
        {

            jump = true;

        }

        //  }


        if (jump)
        {
            rb.AddForce(Vector2.up * 150);
            grounded = false;
            jumpDelay = 20;
        }

        if (!grounded)
            jump = false;

        if(isWalking && grounded && !driving|| isWalking2 && grounded && !driving)
        {
            if (this.GetComponent<AudioSource>().isPlaying == false)
            {
                this.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            this.GetComponent<AudioSource>().Stop();
        }


        foreach (GameObject vanFloor in vanFloors)
        {
            if (vanControl.vanMode == false)
            {
                Physics2D.IgnoreCollision(vanFloor.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), true);
            }
            else
            {
                Physics2D.IgnoreCollision(vanFloor.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), false);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // if (collision2D.collider.tag == "ground" || collision2D.collider.tag == "van floor")
        if (!jump && jumpDelay <= 0)
        {
            if (this.transform.position.y > collision2D.transform.position.y || collision2D.collider.tag == "van floor")
            {
                grounded = true;
            }
        }
    }


    public void inVan()
    {
        this.transform.position = new Vector3(vanControl.transform.position.x + 5f, vanControl.transform.position.y + 0.4f, 0);
        //driving = true;

        rb.isKinematic = true;

        drive_delay --;
        resetTimer = 10;

        if (drive_delay <= 0 && Input.GetKey(KeyCode.Space))
                {
                    driving = false;
                    rb.isKinematic = false; 
                }


    }

    public void Pooper()
    {
        poopTimer -= 1;

        if (poopTimer <= 0)
        {
            poopTimer = 200;
            Instantiate(poop, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity);
            flag.GetComponent<AudioSource>().Play();
        }


    }

}
