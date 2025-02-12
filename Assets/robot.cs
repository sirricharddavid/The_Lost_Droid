using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robot : MonoBehaviour
{
    public float maxdistance;
    public float speed;
    public float excitedspeed;
    public float curiousspeed;
    public float hp;

    private GameObject control;
    public string state = "curious";
    private GameObject player;
    private GameObject canvas;
    private GameObject excitedTarget;
    private float excitedtimer;
    private GameObject nearPoI;
    public float PoIRange;

    private GameObject curiousTarget;
    public GameObject curiousTargetObj;
    private float inspectCounter;

    public GameObject lightStraight;
    public float PoIdistance;
    private float curiousity;
    private float pickable;
    private float pickable2;
    public float maxhp;
    public GameObject robotPart;
    public float throwdamage;
    public float danger;
    private string detectLight;
    private GameObject spaceship;
    public float detectioncdset;
    public float detectiontimerset;
    public float detectioncd;
    public float detectiontimer;
    private GameObject scrapfound;

    public GameObject sfx1;
    public GameObject sfx2;
    public GameObject sfx3;
    private GameObject VolumeManager;
    public GameObject emotion;
    public GameObject emotion1;
    public GameObject emotion2;
    public GameObject emotion3;
    public float emotiontimer;
    private bool start = true;
    // Start is called before the first frame update
    void Start()
    {
        VolumeManager = GameObject.Find("VolumeManager");
        maxhp = 100f;
        player = GameObject.FindGameObjectWithTag("Player");
        control = GameObject.Find("GameController");
        canvas = GameObject.Find("Canvas");
        excitedTarget = GameObject.Find("excitedTarget");
        spaceship = GameObject.Find("spaceship");

        var curiousTargetPos = new Vector3(player.transform.position.x + Random.Range(-maxdistance, maxdistance), player.transform.position.y, player.transform.position.z + Random.Range(-maxdistance, maxdistance));
        curiousTarget = Instantiate(curiousTargetObj, curiousTargetPos, Quaternion.identity);
        curiousity = Random.Range(70f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        if (danger > 0)
            VolumeManager.GetComponent<VolumeManager>().playCombat();
        else
            VolumeManager.GetComponent<VolumeManager>().stopCombat();

        if (start == true)
        {
            canvas.GetComponent<canvasController>().infopaneltext.text = "First of all we should take care of our robot, walk to it to pick it up by pressing 'E', if objects can be picked up they will be highlighted";
            canvas.GetComponent<canvasController>().infopanel.SetActive(true);
        }

        if(emotiontimer > 0)
        {
            emotiontimer -= Time.deltaTime;
            if(emotiontimer < 0)
            {
                emotion.SetActive(false);
                emotion1.SetActive(false);
                emotion2.SetActive(false);
                emotion3.SetActive(false);
            }
        }

        sfx1.GetComponent<AudioSource>().volume = VolumeManager.GetComponent<VolumeManager>().volume/2;
        sfx2.GetComponent<AudioSource>().volume = VolumeManager.GetComponent<VolumeManager>().volume/2;
        sfx3.GetComponent<AudioSource>().volume = VolumeManager.GetComponent<VolumeManager>().volume/2;
        
        canvas.GetComponent<canvasController>().robothpbarval = hp / maxhp;

        if (detectioncd > 0)
            detectioncd -= Time.deltaTime;
        if (danger > 0f)
            danger -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R) && state != "throwready")
        {
            sfx3.GetComponent<AudioSource>().Play(0);
            state = "throwready";
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            hp -= 1;
        }
        if (Input.GetKeyDown(KeyCode.K) && detectioncd <= 0 && state != "scared")
        {
            sfx2.GetComponent<AudioSource>().Play(0);
            detectiontimer = detectiontimerset;
            detectioncd = detectioncdset;
            state = "detecting";
            detectLight = "spaceship";

            emotiontimer = 1f;
            emotion.SetActive(true);
            emotion2.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.L) && detectioncd <= 0 && state != "scared")
        {
            sfx2.GetComponent<AudioSource>().Play(0);
            detectiontimer = detectiontimerset;
            detectioncd = detectioncdset;
            state = "detecting";
            detectLight = "scrap";
            scrapfound = findClosestScrap();

            emotiontimer = 1f;
            emotion.SetActive(true);
            emotion2.SetActive(true);

            if (player.GetComponent<player>().start == 2)
            {
                canvas.GetComponent<canvasController>().infopanel.SetActive(true);
                canvas.GetComponent<canvasController>().infopaneltext.text = "you can also always call your robot to you with pressing 'R' if you want to pick it up, but now let's go and find the scrap";
                player.GetComponent<player>().start = 1;
            }
        }

        if (pickable > 0)
        {           
            pickable -= Time.deltaTime;

            if (state == "dead")
            {
                canvas.GetComponent<canvasController>().CollectTextSet = false;
                robotPart.GetComponent<Renderer>().material.color = new Color32((byte)255, (byte)255, (byte)255, 255);
                player.GetComponent<player>().busyCarry = 0.1f;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    player.GetComponent<player>().robotamount += 1;
                    player.GetComponent<player>().carry = 2; 
                    player.GetComponent<player>().carrying();
                    canvas.GetComponent<canvasController>().CollectTextSet = false;
                    pickable = 0f;
                    transform.position = new Vector3(0f, -100f, 0f);

                    if (start)
                    {
                        canvas.GetComponent<canvasController>().infopanel.SetActive(true);
                        canvas.GetComponent<canvasController>().infopaneltext.text = "you can repair the robot by holding 'R'";
                        start = false;
                    }
                }

                if (pickable <= 0f)
                {
                    robotPart.GetComponent<Renderer>().material.color = new Color32(16, 99, 34, 255);
                    pickable2 = 0;
                }
            }

            if (pickable2 > 0)
            {
                pickable2 -= Time.deltaTime;

                if (state == "throwready")
                {
                    canvas.GetComponent<canvasController>().CollectTextSet = false;
                    robotPart.GetComponent<Renderer>().material.color = new Color32((byte)255, (byte)255, (byte)255, 255);
                    player.GetComponent<player>().busyCarry = 0.1f;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        state = "carried";
                        player.GetComponent<player>().robotamount += 1;
                        player.GetComponent<player>().carry = 2;
                        player.GetComponent<player>().carrying();
                        canvas.GetComponent<canvasController>().CollectTextSet = false;
                        pickable2 = 0f;
                        transform.position = new Vector3(0f, -100f, 0f);
                    }
                }

                if (pickable2 <= 0f)
                {
                    robotPart.GetComponent<Renderer>().material.color = new Color32(16, 99, 34, 255);
                }

            }
        }

        if (hp <= 0)
        {
            state = "dead";
            excitedtimer = 0f;
            inspectCounter = 0f;

            emotiontimer = 1f;
            emotion.SetActive(true);
            emotion3.SetActive(true);
        }
        else
        {
            emotion3.SetActive(false);
        }

        if (curiousity < 100f)
        {
            curiousity += Time.deltaTime;
            if (curiousity > 100f)
                sfx3.GetComponent<AudioSource>().Play(0);
        }
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if(distance > 300f && player.GetComponent<player>().robotamount == 0)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            transform.position = player.transform.position + new Vector3(2f, 0f, 0f);
        }

        if(state == "curious")
        {
            curiousBehavior();
        }

        if (state == "excited")
        {
            excitedBehavior();
        }

        if (state == "scared")
        {
            scaredBehavior();
        }

        if (state == "inspect")
        {
            inspectBehavior(nearPoI);
        }
        else
        {
            lightStraight.SetActive(false);
        }

        if(state == "throwready")
        {
            throwreadyBehavior();
        }

        if(state == "detecting")
        {
            detectiontimer -= Time.deltaTime;
            if (detectLight == "scrap")
            {
                Vector3 direction = (scrapfound.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                lightStraight.SetActive(true);

                distance = Vector3.Distance(player.transform.position, transform.position);
                float distance2 = Vector3.Distance(scrapfound.transform.position, transform.position);

                //if (distance < maxdistance && distance2 > 1f)
                transform.position = Vector3.MoveTowards(transform.position, scrapfound.transform.position, speed * Time.deltaTime);

                if (distance2 < 4f)
                {
                    sfx1.GetComponent<AudioSource>().Play(0);
                    state = "excited";
                    excitedtimer = 5f;

                    emotiontimer = 1f;
                    emotion.SetActive(true);
                    emotion2.SetActive(true);
                }
            }
            if (detectLight == "spaceship")
            {
                Vector3 direction = (spaceship.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                lightStraight.SetActive(true);

                distance = Vector3.Distance(player.transform.position, transform.position);
                float distance2 = Vector3.Distance(spaceship.transform.position, transform.position);

                //if (distance < maxdistance && distance2 > 1f)
                transform.position = Vector3.MoveTowards(transform.position, spaceship.transform.position, speed * Time.deltaTime);

            }
            if (detectiontimer <= 0)
            {
                lightStraight.SetActive(false);
                state = "curious";
            }

            if (danger > 0)
            {
                lightStraight.SetActive(false);
                state = "scared";
            }
        }
    }

    public GameObject findClosestScrap()
    {   
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        control.GetComponent<gamecontrol>().updateArray();
        foreach (GameObject go in control.GetComponent<gamecontrol>().scraps)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public GameObject findClosestPoI()
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in control.GetComponent<gamecontrol>().PoI)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    void throwreadyBehavior()
    {
        pickable2 = 0.1f;
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance > 1f)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    void curiousBehavior()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if(distance > maxdistance)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            float distancecurious = Vector3.Distance(player.transform.position, curiousTarget.transform.position);
            if (distancecurious > maxdistance)
            {
                Destroy(curiousTarget);
                var curiousTargetPos = new Vector3(player.transform.position.x + Random.Range(-maxdistance, maxdistance), player.transform.position.y, player.transform.position.z + Random.Range(-maxdistance, maxdistance));
                curiousTarget = Instantiate(curiousTargetObj, curiousTargetPos, Quaternion.identity);
            }

            Vector3 direction = (curiousTarget.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            transform.position = Vector3.MoveTowards(transform.position, curiousTarget.transform.position, curiousspeed * Time.deltaTime);

            nearPoI = findClosestPoI();
            if (Vector3.Distance(player.transform.position, nearPoI.transform.position) < PoIRange && curiousity >= 100f)
            {
                state = "inspect";
                curiousity = Random.Range(0f, 30f);
                sfx3.GetComponent<AudioSource>().Play(0);

                emotiontimer = 1f;
                emotion.SetActive(true);
                emotion2.SetActive(true);
            }
        }


        if (hp <= 10 || danger > 0)
        {
            state = "scared";
        }
    }

    void excitedBehavior()
    {
        Vector3 direction = (excitedTarget.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        transform.position = Vector3.MoveTowards(transform.position, excitedTarget.transform.position, excitedspeed * Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position, excitedTarget.transform.position, excitedspeed * Time.deltaTime);
        excitedtimer -= Time.deltaTime;
        if (excitedtimer < 0)
        {
            state = "curious";
        }


        if (hp <= 10 || danger > 0)
        {
            state = "scared";
        }
    }

    void scaredBehavior()
    {
        if(hp > 10 && danger <= 0f)
        {
            state = "curious";
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance > 1.5f)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        if (distance < 1f)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -speed * Time.deltaTime);
        }
    }

    void inspectBehavior(GameObject obj)
    {
        float distance = Vector3.Distance(obj.transform.position, transform.position);
        if (distance > PoIdistance && inspectCounter <= 0)
        {

            Vector3 direction = (obj.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            transform.position = Vector3.MoveTowards(transform.position, obj.transform.position, speed * Time.deltaTime);

            distance = Vector3.Distance(obj.transform.position, transform.position);
            if (distance < PoIdistance)
            {
                inspectCounter = 10f;
                lightStraight.SetActive(true);
                GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            }
        }
        else if(inspectCounter > 0)
        {

            Vector3 direction = (obj.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            inspectCounter -= Time.deltaTime;
            if (inspectCounter <= 0)
            {
                state = "excited";
                sfx1.GetComponent<AudioSource>().Play(0);
                excitedtimer = Random.Range(7f, 13f);
                lightStraight.SetActive(false);

                emotiontimer = 1f;
                emotion.SetActive(true);
                emotion1.SetActive(true);
            }
            if(inspectCounter < 7 && inspectCounter > 5)
            {
                lightStraight.SetActive(false);
            }
            if (inspectCounter < 5 && inspectCounter > 3)
            {
                lightStraight.SetActive(true);
            }

            if (inspectCounter < 3 && inspectCounter > 2)
            {
                lightStraight.SetActive(false);
            }
            if (inspectCounter < 2 && inspectCounter > 1)
            {
                lightStraight.SetActive(true);
            }
        }

        distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance > maxdistance*4)
        {
            state = "curious";
            excitedtimer = 0f;
            inspectCounter = 0f;
            lightStraight.SetActive(false);
        }


        if (hp <= 10 || danger > 0)
        {
            state = "scared";
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "curiousRobot")
        {
            Destroy(curiousTarget);
            var curiousTargetPos = new Vector3(player.transform.position.x + Random.Range(-maxdistance, maxdistance), player.transform.position.y, player.transform.position.z + Random.Range(-maxdistance, maxdistance));
            curiousTarget = Instantiate(curiousTargetObj, curiousTargetPos, Quaternion.identity);
        }

        if(state == "throw")
        {
            if (col.gameObject.tag == "turret") 
            {
                col.GetComponent<turret>().hp -= throwdamage;

            }
            if (col.gameObject.tag == "turretsingle")
            {
                col.GetComponent<turretsingle>().hp -= throwdamage;

            }
            if (col.gameObject.tag != "Player" && col.gameObject.tag != "PBullet" && col.gameObject.tag != "spaceship")
            {
                state = "excited";
                sfx1.GetComponent<AudioSource>().Play(0);
                excitedtimer = 5f;

                emotiontimer = 1f;
                emotion.SetActive(true);
                emotion1.SetActive(true);
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")           
        {
            if (col.GetComponent<player>().carry == 0)
            {
                pickable = 0.1f;
            }
        }
    }
}
