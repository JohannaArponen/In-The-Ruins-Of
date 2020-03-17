using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openInventoryandClose : MonoBehaviour{

    public static bool GameIsPause = false;
    public GameObject pauseInventoryUI;
    
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPause) {
                Resume();
            } else {
                OpenIn();
            }
        }
    }

    void Resume() {
        pauseInventoryUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }
    void OpenIn() {
        pauseInventoryUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    public void Test() {
        print("testi");
    }
}
