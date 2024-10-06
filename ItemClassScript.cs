using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClassScript : MonoBehaviour
{
    public enum ItemStates {
        idle,
        picked,
        packaged,
        dropping,
        cooking,
        eating

    }

    public ItemStates currentState;

    private GameObject playerObj;
    private playerScript playerControl;
    private Collider2D playerCollider;
    private Rigidbody2D playerBody;

    private Rigidbody2D rb;

    private GameObject vanObj;
    private Collider2D vanCollider;
    private vanScript vanControl;

    private SpriteRenderer rend;

    private bool dropped = false;
    private bool inTrunk = false;

    private GameObject storageTrigger;
    private Transform STPos;
    private Collider2D STCollider;

    private GameObject storage;
    private StorageLocationScript SCapacity;

    private GameObject[] storageWalls;
    private GameObject[] items;
    private GameObject[] fuelboxes;

    //maybe need to ignore vanwalls

    private GameObject fuelManager;
    private FuelManagerScript fuelControl;

    public bool cooked = false;
    public bool inHand;

    private GameObject stoveObj;

    private GameObject tableObj;
    private GameObject mopObj;
    private GameObject driveObj;
    private GameObject farmObj;
    private GameObject AutoPicker;

    private GameObject NPC1;
    public Sprite cookedMeat;
    private int cookingTimer;

    private GameObject soundManager;
    private AudioSource SMAudio;

    private GameObject eatAnim;
    public int eatTimer;
    private int delay;

    // Start is called before the first frame update
    void Start()
    {
        currentState = ItemStates.idle;

        inHand = false;

        rb = this.GetComponent<Rigidbody2D>();

        playerObj = GameObject.FindWithTag("Player");
        playerControl = playerObj.GetComponent<playerScript>();
        playerCollider = playerObj.GetComponent<Collider2D>();
        playerBody = playerObj.GetComponent<Rigidbody2D>();

        vanObj = GameObject.FindWithTag("van");
        vanCollider = vanObj.GetComponent<Collider2D>();
        vanControl = vanObj.GetComponent<vanScript>();

        rend = this.GetComponent<SpriteRenderer>();
        rend.sortingOrder = 3;

        storageTrigger = GameObject.FindWithTag("storage trigger");
        STPos = storageTrigger.GetComponent<Transform>();
        STCollider = storageTrigger.GetComponent<Collider2D>();

        storage = GameObject.FindWithTag("storage");
        //StoragePos = storage.GetComponent<Transform>();

        storageWalls = GameObject.FindGameObjectsWithTag("storage wall");
        items = GameObject.FindGameObjectsWithTag("item");
        fuelboxes = GameObject.FindGameObjectsWithTag("fuel box");

        SCapacity = storage.GetComponent<StorageLocationScript>();

        fuelManager = GameObject.FindWithTag("fuel manager");
        fuelControl = fuelManager.GetComponent<FuelManagerScript>();

        stoveObj = GameObject.FindWithTag("stove");
        tableObj = GameObject.FindWithTag("table");
        mopObj = GameObject.FindWithTag("mop");
        driveObj = GameObject.FindWithTag("drive trigger");
        farmObj = GameObject.FindWithTag("farm trigger");

        //AutoPicker = GameObject.FindWithTag("auto picker");

        NPC1 = GameObject.FindWithTag("Autopilot NPC");

        stoveObj.GetComponent<SpriteRenderer>().enabled = false;
        cookingTimer = 100;

        soundManager = GameObject.FindWithTag("sound manager");
        SMAudio = soundManager.GetComponent<AudioSource>();

        eatAnim = GameObject.FindWithTag("eating anim");
        eatAnim.GetComponent<SpriteRenderer>().enabled = false;
        eatTimer = 100;

        delay = 10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       //Debug.Log(cookedMeat);

        Physics2D.IgnoreCollision(vanCollider, this.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(STCollider, this.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(mopObj.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());


        switch (currentState) {

            case ItemStates.idle:

                delay = 10;
                rb.isKinematic = false;
                this.GetComponent<Collider2D>().enabled = true;

                dropped = false;
                inHand = false;

                if (inTrunk && vanControl.unloading == false)
                {
                    currentState = ItemStates.packaged;
                }


                if (playerControl.driving == true)
                {
                    Physics2D.IgnoreCollision(playerCollider, this.GetComponent<Collider2D>());

                } else
                {
                    Physics2D.IgnoreCollision(playerCollider, this.GetComponent<Collider2D>(), false);

                    if (Mathf.Abs(playerControl.transform.position.x - this.transform.position.x) <= 1 && Input.GetKey(KeyCode.Space) && playerControl.transform.position.y < this.transform.position.y + 1 && playerControl.transform.position.y > this.transform.position.y && playerControl.carrying == false && playerControl.cooking == false && playerControl.eating == false)
                    {
                        playerControl.carrying = true;
                        currentState = ItemStates.picked;
                        SMAudio.Play();
                    }

                }

                break;



            case ItemStates.picked:

                delay--;
                this.GetComponent<Collider2D>().enabled = false;

                if (!playerControl.fatigued && !playerControl.cleaning)
                {
                    rb.isKinematic = true;
                    this.transform.position = new Vector3(playerControl.transform.position.x, playerControl.transform.position.y + 0.7f, 0);
                    this.transform.rotation = Quaternion.identity;
                    inHand = true;
                

                if (Input.GetKey(KeyCode.Space) && delay <= 0)
                {
                    playerControl.carrying = false;

                    if (this.tag == "fuel box")
                    {
                        if (Mathf.Abs(STPos.position.x - this.transform.position.x) <= 1f && this.transform.position.y < STPos.position.y + 1)
                        {
                            currentState = ItemStates.packaged;
                            fuelControl.Refuel(35);
                            vanObj.GetComponent<AudioSource>().Play();
                        }
                        else
                        {
                            currentState = ItemStates.dropping;
                                SMAudio.Play();
                            }
                    }

                   if (this.tag == "item" || this.tag == "potato")
                    {
                        if (!cooked)
                        {
                            if (Mathf.Abs(stoveObj.transform.position.x - this.transform.position.x) <= 1f && this.transform.position.y > stoveObj.transform.position.y)
                            {
                                currentState = ItemStates.cooking;
                                stoveObj.GetComponent<SpriteRenderer>().enabled = true;
                                stoveObj.GetComponent<AudioSource>().Play();

                                }
                            else
                            {
                                currentState = ItemStates.dropping;
                                    SMAudio.Play();
                                }
                        }


                        if (cooked)
                        {
                            if (Mathf.Abs(tableObj.transform.position.x - this.transform.position.x) <= 1f && this.transform.position.y > tableObj.transform.position.y)
                            {
                                currentState = ItemStates.eating;
                                playerControl.eating = true;
                                eatAnim.GetComponent<SpriteRenderer>().enabled = true;
                                eatAnim.GetComponent<AudioSource>().Play();

                            } else
                            {
                                currentState = ItemStates.dropping;
                                    SMAudio.Play();
                            }

                            //npc 1
                                if (Mathf.Abs(NPC1.transform.position.x - this.transform.position.x) <= 1f && vanControl.vanMode == false && NPC1.GetComponent<AutoPilotNPCScript>().currentState == AutoPilotNPCScript.NPCState.Initiated)
                                {
                                    if (NPC1.GetComponent<AutoPilotNPCScript>().questComplete == false)
                                    {
                                        currentState = ItemStates.packaged;
                                        NPC1.GetComponent<AutoPilotNPCScript>().questComplete = true;
                                    }
                                    else
                                    {
                                        currentState = ItemStates.dropping;
                                    }

                                }  

                            }

                    }


                   if (this.tag == "autodrive upgrade")
                        {
                            if (Mathf.Abs(driveObj.transform.position.x - this.transform.position.x) <= 1f && this.transform.position.y > driveObj.transform.position.y)
                            {
                                currentState = ItemStates.packaged;
                                vanControl.autodriveAcquired = true;
                               playerControl.driving = false;
                                vanControl.AutoDrive = false;
                                SMAudio.Play();
                            }
                            else
                            {
                                currentState = ItemStates.dropping;
                                SMAudio.Play();
                            }

                        }

                   if(this.tag == "metal")
                        {
                            //if used next to engine, add +1 upgrade
                            //make threshold numbers, eg: 3 = lvl 1, 5 = lvl 2 etc.

                            //separate UI bar to indicate upgrade progress


                        }

                   if(this.tag == "seed")
                        {
                            //if used next to farm, activate the planting process
                            if (Mathf.Abs(farmObj.transform.position.x - this.transform.position.x) <= 1f && this.transform.position.y < farmObj.transform.position.y + 1)
                            {
                                currentState = ItemStates.packaged;
                                farmObj.GetComponent<farmScript>().currentState = farmScript.PlantState.Planted;
                                vanObj.GetComponent<AudioSource>().Play();
                            }
                            else
                            {
                                currentState = ItemStates.dropping;
                                SMAudio.Play();
                            }
                        }

                    }
                }
                else
                {
                    currentState = ItemStates.idle;
                    playerControl.carrying = false;
                }


                break;



            case ItemStates.packaged:

                inHand = false;
                    Destroy(gameObject);
                    Destroy(this);

                   break;



            case ItemStates.dropping:

                inHand = false;
                rb.isKinematic = false;
                this.GetComponent<Collider2D>().enabled = true;
                    

                Physics2D.IgnoreCollision(playerCollider, this.GetComponent<Collider2D>(), true);

                if (dropped)
                {
                    currentState = ItemStates.idle;
                }

                break;


            case ItemStates.cooking:

                //timer animation plays or minigame?
               
                
                cookingTimer --;
                rb.isKinematic = true;

                if(cookingTimer > 0)
                {
                    rend.enabled = false;
                    playerControl.cooking = true;
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().sprite = cookedMeat;
                    rend.enabled = true;
                    cooked = true;
                    currentState = ItemStates.dropping;
                    stoveObj.GetComponent<SpriteRenderer>().enabled = false;
                    stoveObj.GetComponent<AudioSource>().Stop();
                    playerControl.cooking = false;
                    rb.isKinematic = false;
                }

                //change cooked to true, can now be eaten


                break;

            case ItemStates.eating:

                eatTimer --;
                this.transform.rotation = Quaternion.identity;

                if (eatTimer > 0)
                {
                    this.transform.position = new Vector3(tableObj.transform.position.x + 0.2f, tableObj.transform.position.y + 0.2f, 0);
                }
                else
                {
                    playerControl.eaten = true;
                    playerControl.eating = false;
                    eatAnim.GetComponent<SpriteRenderer>().enabled = false;
                    Destroy(this);
                    Destroy(gameObject);
                    eatAnim.GetComponent<AudioSource>().Stop();

                }


                break;

        }


    }


    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(currentState == ItemStates.dropping)
        //if (currentState == ItemStates.dropping && collision2D.collider.tag == "ground" || currentState == ItemStates.dropping && collision2D.collider.tag == "van floor" || currentState == ItemStates.dropping && collision2D.collider.tag == "storage wall" || currentState == ItemStates.dropping && collision2D.collider.tag == "item" || currentState == ItemStates.dropping && collision2D.collider.tag == "fuel box")
        {
            dropped = true;

        }

    }

}
