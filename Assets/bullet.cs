using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float timer;
    private float timerset;
    public float dmg;

    void Update()
    {
        if(timerset < timer)
        {
            timerset += 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "turret" && col.gameObject.tag != "Player" && col.gameObject.tag != "robot" && col.gameObject.tag != "PBullet" && col.gameObject.tag != "EBullet" && col.gameObject.tag != "spaceship" && col.gameObject.tag != "scrap" && col.gameObject.tag != "PoI" && col.gameObject.tag != "turretsingle")
            Destroy(gameObject);
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<player>().hp -= dmg;
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "robot" && gameObject.tag == "EBullet")
        {
            col.gameObject.GetComponent<robot>().hp -= dmg;
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "PBullet")
        {
            Destroy(col.gameObject);
        }
    }
}
