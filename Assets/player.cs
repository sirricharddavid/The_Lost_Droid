using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Animator anim;
    Camera cam;

    public float hp;

    //char Movement
    private CharacterController controller;
    private Vector3 playerVelocity;
    public bool groundedPlayer;
    public float playerSpeed = 5.0f;
    public float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    //carrying
    public int carry = 0;
    public GameObject scrapObj;
    public GameObject gunObj;
    public GameObject gunBackObj;
    public GameObject robotObj;

    //combat
    public float shootingCd;
    private float shootingRemCd;
    public GameObject bullet;
    public GameObject shootPoint;
    public float bulletDur;
    public float bulletSpeed;
    private GameObject shootBullet;
    public float bulletDmg;
    private GameObject robot;
    // object detection
    private GameObject canvas;
    public GameObject scrap;
    private GameObject control;
    private float carryset;
    public int scrapamount;
    public int robotamount;
    public float busyCarry;
    public bool robotBuff;

    private Vector3 respawnpos;
    private GameObject gameOverPanel;

    public float maxhp;
    public float repairingvalue;
    public bool repairingstate = false;
    public GameObject minimap;
    public int start = 3;

    // Start is called before the first frame update
    void Start()
    {
        maxhp = hp;
        controller = gameObject.GetComponent<CharacterController>();
        control = GameObject.Find("GameController");
        minimap = GameObject.Find("minimap");
        cam = Camera.main;
        canvas = GameObject.Find("Canvas");
        robot = GameObject.Find("robot");
        gameOverPanel = GameObject.Find("GameOverScreen");
        gameOverPanel.SetActive(false);
        respawnpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (robot.GetComponent<robot>().hp < maxhp && carry == 2)
        {
            if (Input.GetKey(KeyCode.R))
            {
                repairingstate = true;
                repairingvalue += Time.deltaTime * 10;
                canvas.GetComponent<canvasController>().robotRepairing = repairingvalue / 100;

                if (repairingvalue >= 100)
                {
                    robot.GetComponent<robot>().hp = robot.GetComponent<robot>().maxhp;
                    robot.GetComponent<robot>().state = "curious";
                    repairingstate = false;
                    repairingvalue = 0;
                    drop();
                    canvas.GetComponent<canvasController>().robotRepairing = repairingvalue / 100;

                    robot.GetComponent<robot>().emotiontimer = 1f;
                    robot.GetComponent<robot>().emotion.SetActive(true);
                    robot.GetComponent<robot>().emotion1.SetActive(true);

                    if (start == 3)
                    {
                        canvas.GetComponent<canvasController>().infopanel.SetActive(true);
                        canvas.GetComponent<canvasController>().infopaneltext.text = "With our robot we can finally obtain more information like the health values and the minimap, next try looking for scrap to repair the ship. The robot can guide us in the general direction by pressing 'L'";
                        start = 2;
                    }
                }
            }
            else
            {
                repairingvalue = 0;
                canvas.GetComponent<canvasController>().robotRepairing = repairingvalue / 100;
                repairingstate = false;
            }           
        }

        canvas.GetComponent<canvasController>().playerhpbarval = hp / maxhp;
        if (hp > 0)
        {
            if (robot.GetComponent<robot>().danger <= 0 && hp < maxhp)
            {
                hp += Time.deltaTime * 5;
                if (hp > maxhp)
                    hp = maxhp;
            }

            anim.SetBool("isDying", false);
            if (!repairingstate)
            {
            float distance = Vector3.Distance(robot.transform.position, transform.position);
            if (distance < 10f && robot.GetComponent<robot>().state != "dead")
                robotBuff = true;
            else
                robotBuff = false;

            if (busyCarry > 0)
                busyCarry -= Time.deltaTime;
            if (scrapamount > 1)
            {
                scrapamount -= 1;
                Instantiate(scrap, shootPoint.transform.position, transform.rotation);
                control.GetComponent<gamecontrol>().updateArray();
            }
            if (robotamount == 1)
            {
                carry = 2;
                gunBackObj.SetActive(true);
                gunObj.SetActive(false);
                robotObj.SetActive(true);
                if (scrapamount == 1)
                {
                    carry = 1;
                    drop();
                    carry = 2;
                }
            }

            if (carryset > 0)
            {
                carryset -= Time.deltaTime;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    drop();
                }
            }
            cam.transform.position = transform.position + new Vector3(0f, 15f, -8f);
            minimap.transform.position = transform.position + new Vector3(0f, 130f, 0f);

            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            if (shootingRemCd > 0)
            {
                shootingRemCd -= Time.deltaTime;
            }

            if (carry == 0 && Input.GetMouseButton(0))
            {
                anim.SetBool("isShooting", true);
                if (shootingRemCd <= 0)
                {
                    shootingRemCd = shootingCd;
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 directionsetshootpoint = new Vector3(hit.point.x, shootPoint.transform.position.y, hit.point.z);
                        Vector3 directionsetshootpoint2 = directionsetshootpoint - shootPoint.transform.position;
                        Quaternion rotationsetshootpoint = Quaternion.LookRotation(directionsetshootpoint2);
                        transform.rotation = rotationsetshootpoint;
                        Transform objectHit = hit.transform;
                        shootBullet = Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation);
                        shootBullet.GetComponent<Rigidbody>().velocity = shootPoint.transform.TransformDirection(Vector3.forward * bulletSpeed);
                        shootBullet.GetComponent<bullet>().timer = bulletDur;
                        shootBullet.GetComponent<bullet>().dmg = bulletDmg;
                    }
                }        
            }
            else
            {
                anim.SetBool("isShooting", false);
                if (shootingRemCd <= 0)
                {

                    Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    controller.Move(move * Time.deltaTime * playerSpeed);

                    if (move != Vector3.zero)
                    {
                        gameObject.transform.forward = move;
                    }
                }
            }

            if (carry == 2 && robot.GetComponent<robot>().state == "carried" && Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    robot.GetComponent<robot>().state = "throw";
                    Vector3 directionsetshootpoint = new Vector3(hit.point.x, shootPoint.transform.position.y, hit.point.z);
                    Vector3 directionsetshootpoint2 = directionsetshootpoint - shootPoint.transform.position;
                    Quaternion rotationsetshootpoint = Quaternion.LookRotation(directionsetshootpoint2);
                    transform.rotation = rotationsetshootpoint;
                    Transform objectHit = hit.transform;
                    robot.transform.rotation = shootPoint.transform.rotation;
                    robot.transform.position = shootPoint.transform.position;
                    robot.GetComponent<Rigidbody>().velocity = shootPoint.transform.TransformDirection(Vector3.forward * 20f);
                    robotamount -= 1;
                    robot.transform.position = shootPoint.transform.position;
                    carry = 0;
                    gunObj.SetActive(true);
                    gunBackObj.SetActive(false);
                    scrapObj.SetActive(false);
                    robotObj.SetActive(false);
                }
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            if (robotBuff)
                checkpress();
            }
        }
        else
        {
            anim.SetBool("isDying", true);
            if (carry > 0)
                drop();

            gameOverPanel.SetActive(true);

        }

        if (robot.GetComponent<robot>().state != "dead")
            minimap.SetActive(true);
        else
            minimap.SetActive(false);
    }

    void LateUpdate()
    {
        if(hp <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameOverPanel.SetActive(false);
                controller.enabled = false;
                transform.position = respawnpos;
                hp = maxhp;
                robot.transform.position = respawnpos + new Vector3(2f, 0f, 0f);
                controller.enabled = true;
            }
        }

        if(transform.position.y < -100)
        {
            gameOverPanel.SetActive(false);
            controller.enabled = false;
            transform.position = respawnpos;
            hp = maxhp;
            robot.transform.position = respawnpos + new Vector3(2f, 0f, 0f);
            controller.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (hp > 0 && repairingstate == false)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                anim.SetBool("isRunning", true);
            else
                anim.SetBool("isRunning", false);
            if (shootingRemCd > 0)
            {
                anim.SetBool("isRunning", false);
            }

            //if(controller.isGrounded)
            //    anim.SetBool("isJumping", false);
            //else
            //    anim.SetBool("isJumping", true);

        }
        else
        {
            anim.SetBool("isRunning", false);
        }

    }

    public void carrying()
    {
        carryset = 0.2f;
        gunObj.SetActive(true);
        gunBackObj.SetActive(false);
        scrapObj.SetActive(false);
        robotObj.SetActive(false);

        if (carry != 0)
        {
            gunBackObj.SetActive(true);
            gunObj.SetActive(false);
        }
        if(carry == 1)
        {
            if (start == 1)
            {
                canvas.GetComponent<canvasController>().infopanel.SetActive(true);
                canvas.GetComponent<canvasController>().infopaneltext.text = "now let's bring this back to the spaceship, the robot will guide us back if we press 'K'";
                start = 0;
            }
            
            scrapObj.SetActive(true);
        }
        if (carry == 2)
        {
            robotObj.SetActive(true);
        }
    }

    public void drop()
    {
        if(busyCarry <= 0)
        {
            gunObj.SetActive(true);
            gunBackObj.SetActive(false);
            scrapObj.SetActive(false);
            robotObj.SetActive(false);

            if (carry == 1)
            {
                scrapamount -= 1;
                Instantiate(scrap, shootPoint.transform.position, transform.rotation);
                carry = 0;
                control.GetComponent<gamecontrol>().updateArray();
            }
            if (carry == 2)
            {
                robotamount -= 1;
                robot.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                robot.transform.position = shootPoint.transform.position;
                if(robot.GetComponent<robot>().hp > 0)
                {
                    robot.GetComponent<robot>().state = "curious";
                }
                carry = 0;
            }
        }
    }

    void checkpress()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "turret")
            {
                canvas.GetComponent<canvasController>().enemyhp = hit.transform.gameObject.GetComponent<turret>().hp;
                canvas.GetComponent<canvasController>().enemymaxhp = hit.transform.gameObject.GetComponent<turret>().maxhp;
                canvas.GetComponent<canvasController>().enemyname = hit.transform.gameObject.GetComponent<turret>().nameset;
                canvas.GetComponent<canvasController>().enemytimer = 0.1f;
            }

            if (hit.transform.gameObject.tag == "turretsingle")
            {
                canvas.GetComponent<canvasController>().enemyhp = hit.transform.gameObject.GetComponent<turretsingle>().hp;
                canvas.GetComponent<canvasController>().enemymaxhp = hit.transform.gameObject.GetComponent<turretsingle>().maxhp;
                canvas.GetComponent<canvasController>().enemyname = hit.transform.gameObject.GetComponent<turretsingle>().nameset;
                canvas.GetComponent<canvasController>().enemytimer = 0.1f;
            }

            if (hit.transform.gameObject.tag == "patrol")
            {
                canvas.GetComponent<canvasController>().enemyhp = hit.transform.gameObject.GetComponent<patrol>().hp;
                canvas.GetComponent<canvasController>().enemymaxhp = hit.transform.gameObject.GetComponent<patrol>().maxhp;
                canvas.GetComponent<canvasController>().enemyname = hit.transform.gameObject.GetComponent<patrol>().nameset;
                canvas.GetComponent<canvasController>().enemytimer = 0.1f;
            }
        }
    }

}
