using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turretsingle : MonoBehaviour
{
    public string nameset;
    public float hp;
    public float range;
    public float cooldown;
    public float bulletSpeed;
    public float bulletDuration;
    public float rotateSpeed;
    public float bulletDmg;

    public GameObject bullet;
    public GameObject Cannon;
    public GameObject shootPoint;

    public float maxhp;
    private GameObject player;
    private float cooldownTimer;
    private GameObject shootBullet;

    private bool turretShoot;

    public GameObject hpbar;
    private GameObject canvas;
    private GameObject robot;
    // Start is called before the first frame update
    void Start()
    {
        maxhp = hp;
        player = GameObject.FindGameObjectWithTag("Player");
        canvas = GameObject.Find("Canvas");
        robot = GameObject.Find("robot");
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if(hp <= 0)
        {
            Destroy(gameObject);
            canvas.GetComponent<canvasController>().enemytimer = 0f;
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);
        if(distance < range)
        {
            robot.GetComponent<robot>().danger = 3f;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            Cannon.transform.rotation = Quaternion.Slerp(Cannon.transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);

            shootWithoutCheck();
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PBullet")
        {
            Destroy(col.gameObject);
            hp -= col.gameObject.GetComponent<bullet>().dmg;
        }
    }

    void shootWithCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(shootPoint.transform.position, shootPoint.transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if (cooldownTimer <= 0)
                {
                    cooldownTimer = cooldown;         
                    shootBullet = Instantiate(bullet, shootPoint.transform.position, Cannon.transform.rotation);
                    shootBullet.GetComponent<Rigidbody>().velocity = Cannon.transform.TransformDirection(Vector3.forward * bulletSpeed);
                    shootBullet.GetComponent<bullet>().timer = bulletDuration;
                    shootBullet.GetComponent<bullet>().dmg = bulletDmg;
                   
                }
            }
        }
    }

    void shootWithoutCheck()
    {
        
        if (cooldownTimer <= 0 && player.GetComponent<player>().hp > 0)
        {
            cooldownTimer = cooldown;
            
            shootBullet = Instantiate(bullet, shootPoint.transform.position, Cannon.transform.rotation);
            shootBullet.GetComponent<Rigidbody>().velocity = Cannon.transform.TransformDirection(Vector3.forward * bulletSpeed);
            shootBullet.GetComponent<bullet>().timer = bulletDuration;
            shootBullet.GetComponent<bullet>().dmg = bulletDmg;
        }
            
    }
}
