using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCam : MonoBehaviour
{
    public GameObject GameUI, PlayerUI, button;

    // Start is called before the first frame update
    void Start()
    {
        Camera mycam = gameObject.GetComponent<Camera>();
        GameUI.GetComponent<Canvas>().worldCamera = mycam;
        GameUI.GetComponent<Canvas>().planeDistance = 1f;
        PlayerUI.SetActive(false);
        Camera.SetupCurrent(mycam);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        button.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
