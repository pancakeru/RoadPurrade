using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageCollisionScript : MonoBehaviour
{
    private GameObject vanObj;
    private Collider2D vanCollider;
    private vanScript vanControl;

    private GameObject playerObj;
    private Collider2D playerCollider;
    private playerScript playerControl;

    private GameObject[] vanFloors;

    public bool backdoor;

    // Start is called before the first frame update
    void Start()
    {
        vanFloors = GameObject.FindGameObjectsWithTag("van floor");

        vanObj = GameObject.FindWithTag("van");
        vanCollider = vanObj.GetComponent<Collider2D>();
        vanControl = vanObj.GetComponent<vanScript>();

        playerObj = GameObject.FindWithTag("Player");
        playerCollider = playerObj.GetComponent<Collider2D>();
        playerControl = playerObj.GetComponent<playerScript>();

    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject vanfloor in vanFloors)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), vanfloor.GetComponent<Collider2D>());
        }

        Physics2D.IgnoreCollision(vanCollider, this.GetComponent<Collider2D>(), true);

        if (vanControl.unloading == false)
        {
            Physics2D.IgnoreCollision(playerCollider, this.GetComponent<Collider2D>(), true);
        } else
        {
            if(this.backdoor == false)
            {
                Physics2D.IgnoreCollision(playerCollider, this.GetComponent<Collider2D>(), false);
            }


                
        }

    }
}
