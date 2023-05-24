using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScenes : MonoBehaviour {

    public static LoadScenes Instance { private set; get; }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    public void EndGame() {
        SceneManager.LoadScene(2);
    }

    public void WinGame() {
        SceneManager.LoadScene(3);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
