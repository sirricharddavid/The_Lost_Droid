using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject settingsPanel;
    private bool settingsMenu;
    private GameObject VolumeManager;
    public Slider volumeslider;

    public void quit()
    {
        Application.Quit();
    }

    public void play()
    {
        SceneManager.LoadScene("Intro");
    }

    public void settings()
    {
        settingsMenu = !settingsMenu;
        if (settingsMenu)
        {
            MenuPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }
        else
        {
            MenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }

    }

    void Start()
    {
        VolumeManager = GameObject.Find("VolumeManager");
        volumeslider.value = VolumeManager.GetComponent<VolumeManager>().volume;
    }

    void Update()
    {
        VolumeManager.GetComponent<VolumeManager>().volume = volumeslider.value;
    }
}
