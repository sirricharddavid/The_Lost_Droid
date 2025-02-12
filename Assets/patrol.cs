using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrol : MonoBehaviour
{
    public float hp;
    public float speed;
    public float range;
    private string state;
    private GameObject robot;
    private GameObject player;
    public GameObject patrolTargetObj;
    private GameObject patrolTarget;
    public GameObject spawn;
    private GameObject canvas;
    public float maxhp;
    public float dmg;

    public float atkrange;
    public float atkchargetimer;
    private float atkchargetimercurrent;
    private GameObject target;
    public float attacktimer2;
    private float attacktimer2set;
    public float atkcd;
    private float atkcdset;
    public string nameset;
    private float recdmg;
    // Start is called before the first frame update
    void Start()
    {
        maxhp = hp;
        robot = GameObject.Find("robot");
        player = GameObject.FindGameObjectWithTag("Player");
        canvas = GameObject.Find("Canvas");
        state = "patrolling";

        var curiousTargetPos = new Vector3(spawn.transform.position.x + Random.Range(-range, range), spawn.transform.position.y, spawn.transform.position.z + Random.Range(-range, range));
        patrolTarget = Instantiate(patrolTargetObj, curiousTargetPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(recdmg > 0)
        {
            recdmg -= Time.deltaTime;
            if (recdmg <= 0)
                hp = maxhp;
        }
        if (hp < maxhp && spawn.GetComponent<patrolSpawner>().attackset == false)
        {
            spawn.GetComponent<patrolSpawner>().attackset = true;
            spawn.GetComponent<patrolSpawner>().attack = 10f;
            spawn.GetComponent<patrolSpawner>().playaud();
        }

        if (hp <= 0)
            Destroy(gameObject);

        if (atkcdset > 0f)
            atkcdset -= Time.deltaTime;

            if (state == "patrolling")
        {
            float distance;
            if (robot.GetComponent<robot>().state != "dead" && player.GetComponent<player>().carry != 2)
                distance = Vector3.Distance(robot.transform.position, transform.position);
            else
                distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance < range || spawn.GetComponent<patrolSpawner>().attack > 0f)
            {
                if(robot.GetComponent<robot>().state != "dead" && player.GetComponent<player>().carry != 2)
                {
                    if (spawn.GetComponent<patrolSpawner>().attackset == false)
                    {
                        spawn.GetComponent<patrolSpawner>().attackset = true;
                        spawn.GetComponent<patrolSpawner>().attack = 10f;
                        spawn.GetComponent<patrolSpawner>().playaud();
                    }
                    robot.GetComponent<robot>().danger = 3f;
                    Vector3 direction = (robot.transform.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                    if (distance > atkrange)
                        transform.position = Vector3.MoveTowards(transform.position, robot.transform.position, speed * Time.deltaTime);
                    if (distance <= atkrange && atkcdset <= 0f)
                    {
                        state = "attack";
                        target = robot;
                    }
                }
                else
                {
                    if (spawn.GetComponent<patrolSpawner>().attackset == false)
                    {
                        spawn.GetComponent<patrolSpawner>().attackset = true;
                        spawn.GetComponent<patrolSpawner>().attack = 10f;
                        spawn.GetComponent<patrolSpawner>().playaud();
                    }
                    robot.GetComponent<robot>().danger = 3f;
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                    if(distance > atkrange)
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    if (distance <= atkrange && atkcdset <= 0f)
                    {
                        state = "attack";
                        target = player;
                    }
                }
            }
            else
            {
                float distancepatrol = Vector3.Distance(patrolTarget.transform.position, spawn.transform.position);
                if (distancepatrol > range)
                {
                    Destroy(patrolTarget);
                    var curiousTargetPos = new Vector3(spawn.transform.position.x + Random.Range(-range, range), spawn.transform.position.y, spawn.transform.position.z + Random.Range(-range, range));
                    patrolTarget = Instantiate(patrolTargetObj, curiousTargetPos, Quaternion.identity);
                }

                Vector3 direction = (patrolTarget.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                transform.position = Vector3.MoveTowards(transform.position, patrolTarget.transform.position, speed/3 * Time.deltaTime);
            }


            //if (hp <= 10)
            //{
            //    state = "scared";
            //}
        }

        if(state == "attack")
        {
            if (atkchargetimercurrent < atkchargetimer)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            atkchargetimercurrent += Time.deltaTime;
            if(atkchargetimercurrent > atkchargetimer)
            {
                transform.position = Vector3.MoveTowards(transform.position, patrolTarget.transform.position, speed / 2 * Time.deltaTime);
                GetComponent<Rigidbody>().velocity = transform.forward * speed * 2;
                attacktimer2set += Time.deltaTime;
                if (attacktimer2set > attacktimer2)
                {
                    state = "patrolling";
                    GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    atkchargetimercurrent = 0f;
                    attacktimer2set = 0f;
                    atkcdset = atkcd;
                }
            }
        }
    }

    void OnDestroy()
    {
        if(patrolTarget != null)
            Destroy(patrolTarget);
        if (spawn != null)
        {
            spawn.GetComponent<patrolSpawner>().respawning = true;
            spawn.GetComponent<patrolSpawner>().currentPatrolAmount -= 1;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == patrolTarget)
        {
            Destroy(patrolTarget);
            var curiousTargetPos = new Vector3(spawn.transform.position.x + Random.Range(-range, range), spawn.transform.position.y, spawn.transform.position.z + Random.Range(-range, range));
            patrolTarget = Instantiate(patrolTargetObj, curiousTargetPos, Quaternion.identity);
        }

        if (col.gameObject.tag == "PBullet")
        {
            recdmg = 30f;
            Destroy(col.gameObject);
            hp -= col.gameObject.GetComponent<bullet>().dmg;
        }

        if (col.gameObject.tag == "Player" && state == "attack")
        {
            recdmg = 30f;
            col.gameObject.GetComponent<player>().hp -= dmg;
        }

        if (col.gameObject.tag == "robot" && state == "attack")
        {
            recdmg = 30f;
            col.gameObject.GetComponent<robot>().hp -= dmg;
        }
    }
}
