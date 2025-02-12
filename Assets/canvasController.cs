using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class canvasController : MonoBehaviour
{
    public bool RepTextSet = false;
    public float RepBarVal = 0;
    public GameObject RepText;
    public GameObject RepBar;


    public bool CollectTextSet = false;
    public GameObject CollectText;

    public string enemyname;
    public float enemyhp;
    public float enemymaxhp;
    public float enemytimer;

    public GameObject enemypanel;
    public GameObject enemypanelname;
    public GameObject enemypanelhealth;

    public GameObject playerhpbar;
    public float playerhpbarval;
    public GameObject robothpbar;
    public float robothpbarval;

    public GameObject robotInfo;
    public float robotRepairing;
    public GameObject robotRepair;
    public GameObject robotRepairSet;

    public GameObject PauseMenuPanel;
    public GameObject settingsPanel;
    private bool pauseMenu;
    private bool settingsMenu;
    private GameObject VolumeManager;
    public Slider volumeslider;
    public GameObject infopanel;
    public Text infopaneltext;
    public GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        VolumeManager = GameObject.Find("VolumeManager");
        cursor = GameObject.Find("cursor");
        volumeslider.value = VolumeManager.GetComponent<VolumeManager>().volume;
    }

    // Update is called once per frame
    void Update()
    {
        VolumeManager.GetComponent<VolumeManager>().volume = volumeslider.value;

        if (robothpbarval > 0)
            robotInfo.SetActive(true);
        else
            robotInfo.SetActive(false);

        RepText.SetActive(RepTextSet);
        RepBar.GetComponent<Image>().fillAmount = RepBarVal;

        playerhpbar.GetComponent<Image>().fillAmount = playerhpbarval;
        robothpbar.GetComponent<Image>().fillAmount = robothpbarval;

        CollectText.SetActive(CollectTextSet);

        enemypanel.SetActive(false);
        if (enemytimer > 0)
        {
            enemytimer -= Time.deltaTime;
            enemypanel.SetActive(true);
            enemypanelhealth.GetComponent<Image>().fillAmount = enemyhp / enemymaxhp;
            enemypanelname.GetComponent<Text>().text = enemyname;
        }
        if(robotRepairing > 0f)
        {
            robotRepairSet.SetActive(true);
            robotRepair.GetComponent<Image>().fillAmount = robotRepairing;
        }
        else
            robotRepairSet.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu = !pauseMenu;

            if (pauseMenu)
            {
                Time.timeScale = 0f;
                PauseMenuPanel.SetActive(true);
                Cursor.visible = true;
                cursor.SetActive(false);
            }
            else
            {
                Time.timeScale = 1f;
                PauseMenuPanel.SetActive(false);
                settingsPanel.SetActive(false);
                settingsMenu = false;
                Cursor.visible = false;
                cursor.SetActive(true);
            }
        }
    }

    public void quit()
    {
        Application.Quit();
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void resume()
    {
        Time.timeScale = 1f;
        PauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        settingsMenu = false;
        pauseMenu = false;
        Cursor.visible = false;
        cursor.SetActive(true);
    }

    public void settings()
    {
        settingsMenu = !settingsMenu;
        if (settingsMenu)
        {
            PauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }
        else
        {
            PauseMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }
    }
}
