using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveModeScript : MonoBehaviour
{

    private GameObject vanObj;
    private vanScript vanControl;

    private GameObject playerObj;
    private playerScript playerControl;

    private GameObject soundManager;
    private AudioSource SMAudio;

    private int delay;

    // Start is called before the first frame update
    void Start()
    {
        vanObj = GameObject.FindWithTag("van");
        playerObj = GameObject.FindWithTag("Player");

        vanControl = vanObj.GetComponent<vanScript>();
        playerControl = playerObj.GetComponent<playerScript>();

        soundManager = GameObject.FindWithTag("sound manager");
        SMAudio = soundManager.GetComponent<AudioSource>();

        delay = 10;

    }

    // hi nicholas!!
    void FixedUpdate()
    {
        //Debug.Log(playerControl.driving);

        delay--;

        if (vanControl.vanMode && !vanControl.autodriveAcquired)
        {
            if (Mathf.Abs(playerControl.transform.position.x - this.transform.position.x) <= 1 && Input.GetKey(KeyCode.Space) && playerControl.carrying == false && playerControl.driving == false && playerControl.resetTimer <= 0)
            {
                SMAudio.Play();
                //add selection for manual or autopilot

                //if (!vanControl.autodriveAcquired)
                //{
                        
                         playerControl.driving = true;
      


                //}
                //else
                //{


                
            }
        }


        if (vanControl.autodriveAcquired)
        {
            if (Mathf.Abs(playerControl.transform.position.x - this.transform.position.x) <= 1 && Input.GetKey(KeyCode.Space) && playerControl.carrying == false && vanControl.vanMode && delay <= 0)
            {
                SMAudio.Play();

                delay = 10;

                if (!playerControl.driving && !vanControl.AutoDrive)
                {
                    vanControl.AutoDrive = true;
                    if(this.GetComponent<AudioSource>().isPlaying == false)
                    {
                        this.GetComponent<AudioSource>().Play();
                    }
                }


                else if (!playerControl.driving && vanControl.AutoDrive)
                {
                    vanControl.AutoDrive = false;
                    playerControl.driving = true;

                }

            }
        }

        if(vanControl.AutoDrive == false && !playerControl.driving)
        {
            this.GetComponent<AudioSource>().Stop();
        }



    }
}
