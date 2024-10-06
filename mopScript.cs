using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mopScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject playerObj;
    private Transform playerPos;
    private playerScript playerControl;
    private Rigidbody2D playerBody;

    private GameObject[] vanWalls;
    private GameObject[] storageWalls;
    private GameObject vanObj;
    private Collider2D vanCollider;
    private Collider2D playerCollider;

    private GameObject[] items;
    private GameObject[] fuels;

    private GameObject mopHome;
    private Transform mopHomePos;

    public bool inUse = false;

    public Sprite mopLeft;
    public Sprite mopRight;
    private SpriteRenderer mopAppearance;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        playerPos = playerObj.GetComponent<Transform>();
        playerControl = playerObj.GetComponent<playerScript>();
        playerBody = playerObj.GetComponent<Rigidbody2D>();
        playerCollider = playerObj.GetComponent<Collider2D>();

        vanWalls = GameObject.FindGameObjectsWithTag("van floor");
        storageWalls = GameObject.FindGameObjectsWithTag("storage wall");
        vanObj = GameObject.FindWithTag("van");
        vanCollider = vanObj.GetComponent<Collider2D>();

        items = GameObject.FindGameObjectsWithTag("item");
        fuels = GameObject.FindGameObjectsWithTag("fuel box");

        mopHome = GameObject.FindWithTag("mop trigger");
        mopHomePos = mopHome.GetComponent<Transform>();

        mopAppearance = this.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(inUse); 

        Physics2D.IgnoreCollision(vanCollider, this.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(playerCollider, this.GetComponent<Collider2D>());

        foreach (GameObject vanfloor in vanWalls)
        {
            Physics2D.IgnoreCollision(vanfloor.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        }

        foreach (GameObject storagewall in storageWalls)
        {
            Physics2D.IgnoreCollision(storagewall.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        }

        //foreach (GameObject item in items)
        //{
        //    Physics2D.IgnoreCollision(item.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        //}

        //foreach (GameObject fuel in fuels)
        //{
        //    Physics2D.IgnoreCollision(fuel.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
        //}

      
        
        if (inUse)
        {
            playerControl.cleaning = true;
            this.transform.position = new Vector3(this.transform.position.x, playerPos.position.y, 0);


            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W))
            {
                this.transform.position = new Vector3(playerPos.position.x - 0.8f, playerPos.position.y, 0);
                mopAppearance.sprite = mopLeft;
            }

            if (Input.GetKey(KeyCode.D))
            {
                this.transform.position = new Vector3(playerPos.position.x + 0.8f, playerPos.position.y, 0);
                mopAppearance.sprite = mopRight;
            }
            
        } else
        {
            this.transform.position = new Vector3(mopHomePos.position.x+0.1f, mopHomePos.position.y + 0.15f, 0);
            playerControl.cleaning = false;
            mopAppearance.sprite = mopLeft;
        }

    }
}
