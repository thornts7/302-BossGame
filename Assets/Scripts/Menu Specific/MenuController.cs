using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Button Play;
    public Button Exit;
    void Start()
    {
        Play.onClick.AddListener(() => {
            SceneManager.LoadScene(1);
        });
        Exit.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
