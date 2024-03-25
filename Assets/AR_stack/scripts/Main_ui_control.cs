using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main_ui_control : MonoBehaviour
{
    public GameObject game_obj_start_btn;

    public Text text_record;

    void Awake()
    {
        new Config();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnEnable()
    {
        this.text_record.text = "Record" + ": " + Config.record;
    }

    public void on_btn_choose(int num)
    {
        this.game_obj_start_btn.SetActive(false);
        SceneManager.LoadSceneAsync("ar_stack");
    }
}
