using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bear.j.easy_dialog;

public class Stack_main_control : MonoBehaviour
{
    //单例模式 Singleton mode
    public static Stack_main_control instance;

    //存放的颜色序列 Stored color sequence
    public static string[] colors = {
        "#767c6b", "#888e7e", "#5a544b", "#56564b", "#56564b", "#494a41", "#6b6f59", "#474b42", "#333631", "#5b6356", "#726250", "#9d896c", "#94846a", "#897858", "#716246","#cbb994",
        "#d6c6af", "#bfa46f", "#9e9478", "#a59564", "#715c1f", "#c7b370", "#dcd3b2", "#a19361", "#8f8667", "#887938", "#6a5d21", "#918754", "#a69425", "#ada250", "#938b4b", "#8c8861", "#a1a46d", "#726d40",
        "#928c36","#dccb18", "#d7cf3a", "#c5c56a", "#c3d825", "#b8d200", "#e0ebaf", "#d8e698", "#c7dc68", "#99ab4e", "#7b8d42", "#69821b", "#aacf53", "#b0ca71", "#b9d08b", "#839b5c", "#cee4ae", "#82ae46",
        "#a8c97f", "#9ba88d","#c8d5bb","#c1d8ac" };

    //Game_stau
    public Game_stau game_statu;

    public GameObject game_obj_parent;

    public GameObject game_obj_last_block;

    public GameObject prefab_block;


    public Text text_score;
    public int num_score;

    public GameObject game_obj_game_start;

    public GameObject game_obj_game_over;

    public Text text_record;

    void Awake()
    {
        Stack_main_control.instance = this;
    }

    void Start()
    {
        this.text_score = GameObject.FindGameObjectWithTag("text_score").GetComponent<Text>();
        this.game_obj_game_start = GameObject.FindGameObjectWithTag("game_obj_game_start");
        this.game_obj_game_over = GameObject.FindGameObjectWithTag("game_obj_game_over");

        this.change_to_gaming();

        this.text_record.text = "Record" + ": " + Config.record;
    }

    void Update()
    {

    }


    public void creat_new_block()
    {
        GameObject block = Instantiate(this.prefab_block);
        block.transform.parent = this.game_obj_parent.transform;

        this.num_score++;
        if (this.text_score != null)
            this.text_score.text = this.num_score + "";
    }

    //change to game start
    public void change_to_game_start()
    {
        if (this.game_obj_game_over != null)
            this.game_obj_game_over.SetActive(false);
        if (this.game_obj_game_start != null)
            StartCoroutine(Canvas_grounp_fade.show(this.game_obj_game_start));

        this.num_score = 0;
        if (this.text_score != null)
            this.text_score.text = this.num_score + "";

        this.game_statu = Game_stau.game_start;
    }

    public void change_to_gaming()
    {
        if (this.game_obj_game_start != null)
            StartCoroutine(Canvas_grounp_fade.hide(this.game_obj_game_start));
        if (this.game_obj_game_over != null)
            StartCoroutine(Canvas_grounp_fade.hide(this.game_obj_game_over));

        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        for (int i = 0; i < blocks.Length; i++)
        {
            Destroy(blocks[i]);
        }

        this.num_score = -1;

        this.game_obj_last_block = GameObject.FindGameObjectWithTag("block_0");

        this.creat_new_block();

        this.game_statu = Game_stau.gaming;
    }

    public void change_to_game_over()
    {
        if (this.game_obj_game_start != null)
            this.game_obj_game_start.SetActive(false);
        if (this.game_obj_game_over != null)
            StartCoroutine(Canvas_grounp_fade.show(this.game_obj_game_over));

        #region 
        if (this.num_score > Config.record)
        {
            PlayerPrefs.SetInt("record", this.num_score);
            Config.record = this.num_score;
        }
        this.text_record.text = "Record" + ": " + Config.record;
        #endregion

        this.game_statu = Game_stau.game_over;

        Destory_block_under_ground[] scripts = this.GetComponentsInChildren<Destory_block_under_ground>(true);
        for (int i = 0; i < scripts.Length; i++)
        {
            if (scripts[i].gameObject.activeSelf == false)
                scripts[i].gameObject.SetActive(true);
        }
    }

    public void change_to_game_again()
    {
        StartCoroutine(Canvas_grounp_fade.hide(this.game_obj_game_start));
        StartCoroutine(Canvas_grounp_fade.hide(this.game_obj_game_over));

        this.creat_new_block();

        this.game_statu = Game_stau.gaming;
    }

    //event
    #region 
    public void on_start_game_btn()
    {
        Main_control.instance.on_reset_btn();
    }

    public void on_reset_btn()
    {
        Main_control.instance.on_reset_btn();
    }

    public void on_back_btn()
    {
        Main_control.instance.on_back_btn();
    }

    #endregion



}

public enum Game_stau
{
    game_start,
    gaming,
    game_over
}
