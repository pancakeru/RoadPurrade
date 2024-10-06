using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationScript : MonoBehaviour
{
    private Animator playerAnims;
    public bool isRunning;
    public bool isIdle;
    public bool runLeft;
    public bool idleRight;
    private playerScript playerControl;
    public bool jumpRight;
    public bool lookLeft = true;
    public AudioClip walkSound;
    private AudioSource playerSounds;

    // Start is called before the first frame update
    void Start()
    {

        playerAnims = this.GetComponent<Animator>();
        isRunning = playerAnims.GetBool("running");
        isIdle = playerAnims.GetBool("idle");
        runLeft = playerAnims.GetBool("run left");
        idleRight = playerAnims.GetBool("idle right");
        playerControl = this.GetComponent<playerScript>();
        jumpRight = playerAnims.GetBool("jumping");
        playerSounds = this.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerControl.sleeping == false)
        {
            if (Input.GetKey(KeyCode.D))
            {
                lookLeft = true;
            }

            if (Input.GetKey(KeyCode.A))
            {
                lookLeft = false;
            }

            if (Input.GetKey(KeyCode.D) && playerControl.grounded && !playerControl.driving)
            {


                playerAnims.SetBool("running", true);
            }
            else
            {
                playerAnims.SetBool("running", false);
            }


            if (Input.GetKey(KeyCode.A) && playerControl.grounded && !playerControl.driving)
            {

                playerAnims.SetBool("run left", true);
            }
            else
            {
                playerAnims.SetBool("run left", false);
            }


            if (Input.GetKeyUp(KeyCode.A) && !playerControl.driving)
            {
                playerAnims.SetBool("idle", true);
                playerAnims.SetBool("idle right", false);
            }

            if (Input.GetKeyUp(KeyCode.D) && !playerControl.driving)
            {
                playerAnims.SetBool("idle right", true);
                playerAnims.SetBool("idle", false);
            }


            if (Input.GetKeyDown(KeyCode.W) && lookLeft && !playerControl.driving)
            {
                playerAnims.SetBool("jumping", true);
                playerAnims.SetBool("running", false);
            }
            else
            {
                playerAnims.SetBool("jumping", false);
            }

            if (Input.GetKeyDown(KeyCode.W) && !lookLeft && !playerControl.driving)
            {
                playerAnims.SetBool("jump left", true);
                playerAnims.SetBool("run left", false);
            }
            else
            {
                playerAnims.SetBool("jump left", false);
            }

            if (playerControl.driving)
            {
                playerAnims.SetBool("idle right", true);
                playerAnims.SetBool("idle", false);
            }

        }
        //playerSounds.clip = walkSound;
        //playerSounds.Play();


    }
}
