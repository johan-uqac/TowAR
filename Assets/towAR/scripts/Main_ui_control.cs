using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main_ui_control : MonoBehaviour
{
    // Start button GameObject
    public GameObject game_obj_start_btn;

    // Record text
    public Text text_record;

    // Awake is called before Start
    void Awake()
    {
        // Initialize configuration
        new Config();

        // Prevent screen from sleeping
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Called when the GameObject becomes enabled and active
    void OnEnable()
    {
        // Update record text when enabled
        this.text_record.text = "Record" + ": " + Config.record;
    }

    // Button click event handler
    public void on_btn_choose(int num)
    {
        // Deactivate start button
        this.game_obj_start_btn.SetActive(false);

        // Load AR stack scene
        SceneManager.LoadSceneAsync("ar_stack");
    }
}
