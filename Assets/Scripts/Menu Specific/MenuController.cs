using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Button Play;
    public Button Menu;
    public Button Exit;
    void Start()
    {
        if (Play != null)
        {
            Play.onClick.AddListener(() => {
                SceneManager.LoadScene(1);
            });
        }
        if (Menu != null)
        {
            Menu.onClick.AddListener(() => {
                SceneManager.LoadScene(0);
            });
        }
        Exit.onClick.AddListener(() => {
            Application.Quit();
        });

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
