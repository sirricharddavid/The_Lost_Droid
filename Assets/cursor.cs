using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour
{
    //public Texture2D cursorArrow;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.SetCursor(cursorArrow, Vector3.zero, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        transform.position = Input.mousePosition;
    }
}
