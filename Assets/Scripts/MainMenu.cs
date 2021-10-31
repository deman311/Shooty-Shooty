using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startBtn, exitBtn, controlsBtn;
    public GameObject controlsText;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        startBtn.onClick.AddListener(delegate
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Game");
        });

        exitBtn.onClick.AddListener(delegate
        {
            Application.Quit();
        });

        controlsBtn.onClick.AddListener(delegate
        {
            bool isActive = controlsText.activeInHierarchy;
            controlsText.SetActive(!isActive);
        });
    }
}
