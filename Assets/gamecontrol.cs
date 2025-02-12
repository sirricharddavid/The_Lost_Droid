using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamecontrol : MonoBehaviour
{
    public GameObject[] scraps;
    public GameObject[] PoI;

    // Start is called before the first frame update
    void Start()
    {
        updateArray();
        //scraps = GameObject.FindGameObjectsWithTag("scrap");
        //PoI = GameObject.FindGameObjectsWithTag("PoI");
    }

    // Update is called once per frame
    public void updateArray()
    {
        scraps = GameObject.FindGameObjectsWithTag("scrap");
        PoI = GameObject.FindGameObjectsWithTag("PoI");
    }
}
