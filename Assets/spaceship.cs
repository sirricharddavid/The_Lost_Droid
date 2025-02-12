using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceship : MonoBehaviour
{
    public float repairValue;
    public float maxRepValue;

    public float canRepair = 0f;
    private GameObject player;
    public GameObject SpaceshipRepaired;
    public GameObject SpaceshipBroken;

    private GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if(canRepair > 0f)
        {
            canRepair -= Time.deltaTime;
            canvas.GetComponent<canvasController>().RepTextSet = true;
            player.GetComponent<player>().busyCarry = 0.1f;
            if (Input.GetKeyDown(KeyCode.E)){
                repairSpaceship();
                canvas.GetComponent<canvasController>().infopanel.SetActive(false);
            }
        }
        else
        {          
            canvas.GetComponent<canvasController>().RepTextSet = false;
        }
        canvas.GetComponent<canvasController>().RepBarVal = repairValue / maxRepValue;
    }
    
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.GetComponent<player>().carry == 1)
            {
                canRepair = 0.1f;
            }
        }
    }

    void repairSpaceship()
    {
        repairValue += 1;
        player.GetComponent<player>().scrapamount -= 1;
        player.GetComponent<player>().carry = 0;
        player.GetComponent<player>().carrying();
        canvas.GetComponent<canvasController>().RepTextSet = false;
        if (repairValue < maxRepValue)
        {
            SpaceshipBroken.SetActive(true);
            SpaceshipRepaired.SetActive(false);
        }
        else
        {
            SpaceshipBroken.SetActive(false);
            SpaceshipRepaired.SetActive(true);
        }
    }
}
