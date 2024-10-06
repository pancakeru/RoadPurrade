using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelManagerScript : MonoBehaviour
{
    public Image FuelBar;
    public float FuelLevel = 100f;

    private GameObject vanObj;
    private vanScript vanFuel;

    private GameObject minigameManager;
    private minigameManagerScript MGMScript;

    // Start is called before the first frame update
    void Start()
    {
        vanObj = GameObject.FindWithTag("van");
        vanFuel = vanObj.GetComponent<vanScript>();

    }

    // Update is called once per frame
    void Update()
    { 

        if (vanFuel.vanMode == true && FuelLevel > 0)
            vanFuel.hasFuel = true;
        else
            vanFuel.hasFuel = false;

    }


    public void UseFuel(float UseAmount)
    {
        //UseAmount = 5;
        FuelLevel -= UseAmount;
        FuelBar.fillAmount = FuelLevel / 100f;
    }

    public void Refuel(float RefuelAmount)
    {
        //RefuelAmount = 15;
        FuelLevel += RefuelAmount;
        FuelLevel = Mathf.Clamp(FuelLevel, 0, 100);

        FuelBar.fillAmount = FuelLevel / 100f;
    }
}
