using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStatusScript : MonoBehaviour
{

    private GameObject playerObj;
    private playerScript playerControl;

    private bool createdHunger = false;
    private bool createdSleepy = false;
    private bool createdStinky = false;

    public GameObject notif;
    public GameObject sleepNotif;
    public GameObject stinkNotif;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        playerControl = playerObj.GetComponent<playerScript>();

        //notif = GameObject.FindWithTag("notif");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControl.needFood && !createdHunger)
        {
            Instantiate(notif, new Vector3(playerControl.transform.position.x - 0.3f, playerControl.transform.position.y + 1.2f, 0), Quaternion.identity);
            createdHunger = true;
        }

        if (playerControl.hungerLevel == 0)
        {
            createdHunger = false;
        }


        if (playerControl.needSleep && !createdSleepy)
        {
            Instantiate(sleepNotif, new Vector3(playerControl.transform.position.x, playerControl.transform.position.y + 1.2f, 0), Quaternion.identity);
            createdSleepy = true;
        }

        if (playerControl.tiredLevel == 0)
        {
            createdSleepy = false;
        }

        if (playerControl.needShower && !createdStinky)
        {
            Instantiate(stinkNotif, new Vector3(playerControl.transform.position.x + 0.3f, playerControl.transform.position.y + 1.2f, 0), Quaternion.identity);
            createdStinky = true;
        }

        if (playerControl.stinkLevel == 0)
        {
            createdStinky = false;
        }


    }
}
