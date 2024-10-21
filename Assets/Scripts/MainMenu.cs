using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool changeRoom = false;

    void Update()
    {
        if (changeRoom)
        {
            foreach (var go in GetDontDestroyOnLoadObjects())
            {
                if(go.name.Equals("PauseMenu")) go.SetActive(true);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void DisableMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Exit()
    {
        Application.Quit();
    }
    
    public GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            Object.DontDestroyOnLoad( temp );
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate( temp );
            temp = null;
 
            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if( temp != null )
                Object.DestroyImmediate( temp );
        }
    }
}
