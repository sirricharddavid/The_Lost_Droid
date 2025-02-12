using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrolSpawner : MonoBehaviour
{
    public float attackrange;
    public float hp;
    public float speed;
    public float PatrolAmount;
    public float currentPatrolAmount;
    private GameObject patrol;
    public GameObject patrolObj;
    public float range;
    public float respawntimer;
    private float respawntimerset;
    public bool respawning;
    public float dmg;
    public string nameset;
    public float attack;
    public bool attackset;
    public AudioSource sfx1;
    public AudioSource sfx2;
    public AudioSource sfx3;
    public AudioSource sfx4;
    private int selaud;
    private GameObject VolumeManager;
    // Start is called before the first frame update
    void Start()
    {
        VolumeManager = GameObject.Find("VolumeManager");
        while (currentPatrolAmount < PatrolAmount)
        {
            Vector3 patrolloc = new Vector3(transform.position.x + Random.Range(-range, range), transform.position.y, transform.position.z + Random.Range(-range, range));
            patrol = Instantiate(patrolObj, patrolloc, Quaternion.identity);
            patrol.GetComponent<patrol>().spawn = gameObject;
            patrol.GetComponent<patrol>().range = attackrange;
            patrol.GetComponent<patrol>().hp = hp;
            patrol.GetComponent<patrol>().speed = speed;
            patrol.GetComponent<patrol>().dmg = dmg;
            patrol.GetComponent<patrol>().nameset = nameset;
            currentPatrolAmount += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        sfx1.volume = VolumeManager.GetComponent<VolumeManager>().volume / 2;
        sfx2.volume = VolumeManager.GetComponent<VolumeManager>().volume / 2;
        sfx3.volume = VolumeManager.GetComponent<VolumeManager>().volume / 2;
        sfx4.volume = VolumeManager.GetComponent<VolumeManager>().volume / 2;

        if (attack > 0f)
        {
            attack -= Time.deltaTime;
            if (attack <= 0f)
                attackset = false;
        }
        
        if (respawning)
        {
            respawntimerset += Time.deltaTime;
            if(respawntimerset > respawntimer)
            {
                respawntimerset = 0;
                respawning = false;
                while (currentPatrolAmount < PatrolAmount)
                {
                    Vector3 patrolloc = new Vector3(transform.position.x + Random.Range(-range, range), transform.position.y, transform.position.z + Random.Range(-range, range));
                    patrol = Instantiate(patrolObj, patrolloc, Quaternion.identity);
                    patrol.GetComponent<patrol>().spawn = gameObject;
                    patrol.GetComponent<patrol>().range = attackrange;
                    patrol.GetComponent<patrol>().hp = hp;
                    patrol.GetComponent<patrol>().speed = speed;
                    patrol.GetComponent<patrol>().dmg = dmg;
                    patrol.GetComponent<patrol>().nameset = nameset; 
                    currentPatrolAmount += 1;
                }
            }

        }
    }

    public void playaud()
    {
        selaud = Random.Range(0, 4);
        if (selaud == 0)
            sfx1.Play(0);
        if (selaud == 1)
            sfx2.Play(0);
        if (selaud == 2)
            sfx3.Play(0);
        if (selaud == 3)
            sfx4.Play(0);
    }
}
