using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class returnBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(delegate
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
        });
    }
}
