using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vanScript : MonoBehaviour
{
    private float moveSpeed = 1.5f;
    private float maxSpeed = 5;
    private Rigidbody2D rb;
    public bool vanMode = false;
    public GameObject player;
    public playerScript playerControl;
    public Collider2D playerCollide;
    public SpriteRenderer playerSprite;
    public bool hasFuel;

    public bool unloading = false;

    private GameObject fuelManager;
    private FuelManagerScript FMScript;

    private GameObject[] vanFloors;
    public GameObject poop;

    private int PoopTimer = 5000;
    public bool autodriveAcquired = false;
    public bool AutoDrive = false;

    private GameObject driveTrigger;
    private AudioSource driveAudio;

    private GameObject flag;
    private float multiplier;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        playerControl = player.GetComponent<playerScript>();
        playerCollide = player.GetComponent<Collider2D>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        vanMode = false;
        hasFuel = true;

        fuelManager = GameObject.FindWithTag("fuel manager");
        FMScript = fuelManager.GetComponent<FuelManagerScript>();

        vanFloors = GameObject.FindGameObjectsWithTag("van floor");
        driveTrigger = GameObject.FindWithTag("drive trigger");
        driveAudio = driveTrigger.GetComponent<AudioSource>();

        flag = GameObject.FindWithTag("flag pic");

        multiplier = 5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log(playerControl.driving);

        poopGeneration();
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        foreach (GameObject vanFloors in vanFloors)
        {

            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), vanFloors.GetComponent<Collider2D>());
        }

        Physics2D.IgnoreCollision(playerCollide.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());

        
            if (vanMode)
            {
                // playerControl.inVan();

                //playerSprite.sortingOrder = -10;



                if (playerControl.driving)
                {
                    AutoDrive = false;

                    playerControl.inVan();

                    if (Input.GetKey(KeyCode.D) && hasFuel)
                    {
                        rb.AddForce(Vector2.right * moveSpeed * multiplier, ForceMode2D.Force);
                        FMScript.UseFuel(0.008f * multiplier);
                            if(driveAudio.isPlaying == false)
                            {
                                 driveAudio.Play();
                            }

                    }

                    if (Input.GetKey(KeyCode.A) && hasFuel)
                    {
                        rb.AddForce(Vector2.left * moveSpeed * multiplier, ForceMode2D.Force);
                        FMScript.UseFuel(0.008f * multiplier);

                         if (driveAudio.isPlaying == false)
                         {
                             driveAudio.Play();
                         }
                    }

                    if (rb.velocity.magnitude > maxSpeed)
                    {
                        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
                    }

                    //if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                    if(rb.velocity.magnitude == 0)
                     {
                         driveAudio.Stop();
                     }

                }

                else
                {
                    playerSprite.sortingOrder = 1;
                    playerControl.driving = false;

                }


            if (!playerControl.driving && !AutoDrive)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                driveAudio.Stop();
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }


            if (AutoDrive && !playerControl.driving)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                if (hasFuel)
                {
                    rb.AddForce(Vector2.right * moveSpeed * 1.5f * multiplier, ForceMode2D.Force);
                    FMScript.UseFuel(0.004f * multiplier);
                } else
                {
                    AutoDrive = false;
                }

                if (rb.velocity.magnitude > maxSpeed / 2.3f)
                {
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed / 2);
                }

            }

           
        }


       

        


        if (!vanMode)
        {
            AutoDrive = false;
            playerControl.driving = false;
        }

    }

    public void poopGeneration()
    {
        PoopTimer -= 1;
        

        if(PoopTimer <= 0)
        {
            PoopTimer = 5000;
            for (var i = 0; i < 3; i++)
            {
                Instantiate(poop, new Vector3(Random.Range(this.transform.position.x - 6f, this.transform.position.x + 4f), Random.Range(this.transform.position.y - 1f, this.transform.position.y + 3.5f), 0), Quaternion.identity);
                flag.GetComponent<AudioSource>().Play();
            }

        }

    }

}





