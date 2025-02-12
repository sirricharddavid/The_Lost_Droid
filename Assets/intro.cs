using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
    private float introtimer;
    public GameObject scene1;
    public GameObject scene2;
    public GameObject scene3;

    void Update()
    {
        introtimer += Time.deltaTime;
        if (introtimer > 8f)
            scene3.SetActive(false);
        if (introtimer > 16f)
            scene2.SetActive(false);
        if (introtimer > 24f)
            SceneManager.LoadScene("Main");
    }
}
