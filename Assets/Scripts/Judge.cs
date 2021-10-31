using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{

    public GameObject bluewin, redwin;
    public GameObject blueteam, redteam;
    public GameObject button;
    public GameObject winSmoke;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // specific
        if (blueteam.transform.childCount == 0)
        {
            redwin.SetActive(true);
            winSmoke.GetComponent<Renderer>().material.color = Color.red;
        }
        if (redteam.transform.childCount == 0)
        {
            bluewin.SetActive(true);
            winSmoke.GetComponent<Renderer>().material.color = Color.blue;
        }

        if(redteam.transform.childCount == 0 || blueteam.transform.childCount == 0) // if any
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            button.SetActive(true);
            winSmoke.SetActive(true);
        }
    }
}
