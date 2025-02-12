using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrap : MonoBehaviour
{
    private GameObject canvas;
    private GameObject player;

    private float canCollect;
    public GameObject scrapPart1;
    public GameObject scrapPart2;
    public GameObject scrapPart3;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (canCollect > 0f)
        {
            player.GetComponent<player>().busyCarry = 0.1f;
            scrapPart1.GetComponent<Renderer>().material.color = new Color32((byte)255, (byte)255, (byte)255, 255);
            scrapPart2.GetComponent<Renderer>().material.color = new Color32((byte)255, (byte)255, (byte)255, 255);
            scrapPart3.GetComponent<Renderer>().material.color = new Color32((byte)255, (byte)255, (byte)255, 255);
            canCollect -= Time.deltaTime;
            //canvas.GetComponent<canvasController>().CollectTextSet = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                player.GetComponent<player>().scrapamount += 1;
                player.GetComponent<player>().carry = 1;
                player.GetComponent<player>().carrying();
                //canvas.GetComponent<canvasController>().CollectTextSet = false;
                Destroy(gameObject);
            }
            if(canCollect <= 0f)
            {
                scrapPart1.GetComponent<Renderer>().material.color = new Color32(200, 200, 200, 255);
                scrapPart2.GetComponent<Renderer>().material.color = new Color32(132, 121, 101, 100);
                scrapPart3.GetComponent<Renderer>().material.color = new Color32(132, 121, 101, 100);
                //canvas.GetComponent<canvasController>().CollectTextSet = false;
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.GetComponent<player>().carry == 0)
            {
                canCollect = 0.1f;
            }
        }
    }
}
