using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class excitedRobotTarget : MonoBehaviour
{

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        gameObject.transform.Rotate(0, Time.deltaTime * -160, 0);
    }
}
