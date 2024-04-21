using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stack_main_control : MonoBehaviour
{
    // Singleton instance
    public static Stack_main_control instance;

    // Array of colors for blocks
    public static string[] colors = {
        "#FF4500", "#FFD700", "#FF69B4", "#00FFFF", "#7FFF00", "#FF1493", "#00FF7F", "#FF6347", "#FF00FF", "#FFFF00"
    };

    // Current game status
    public Game_status game_statu;

    // Parent object for blocks
    public GameObject game_obj_parent;

    // Reference to the last block placed
    public GameObject game_obj_last_block;

    // Prefab for blocks
    public GameObject prefab_block;

    // UI elements
    public Text text_score;
    public Text text_record;

    // Current score
    public int num_score;

    // Game start and game over UI elements
    public GameObject game_obj_game_start;
    public GameObject game_obj_game_over;

    // Awake method to set up singleton instance
    void Awake()
    {
        Stack_main_control.instance = this;
    }

    // Start method to initialize UI and game
    void Start()
    {
        // Initialize UI elements
        this.text_score = GameObject.FindGameObjectWithTag("text_score").GetComponent<Text>();
        this.game_obj_game_start = GameObject.FindGameObjectWithTag("game_obj_game_start");
        this.game_obj_game_over = GameObject.FindGameObjectWithTag("game_obj_game_over");

        // Start the game
        this.change_to_gaming();

        // Update record text
        this.text_record.text = "Record" + ": " + Config.record;
    }

    // Method to create a new block
    public void creat_new_block()
    {
        GameObject block = Instantiate(this.prefab_block);
        block.transform.parent = this.game_obj_parent.transform;

        // Increment score
        this.num_score++;
        if (this.text_score != null)
            this.text_score.text = this.num_score + "";
    }

    // Method to change to game start state
    public void change_to_game_start()
    {
        // Hide game over UI and show game start UI
        if (this.game_obj_game_over != null)
            this.game_obj_game_over.SetActive(false);
        if (this.game_obj_game_start != null)
            StartCoroutine(Canvas_grounp_fade.show(this.game_obj_game_start));

        // Reset score
        this.num_score = 0;
        if (this.text_score != null)
            this.text_score.text = this.num_score + "";

        // Change game status
        this.game_statu = Game_status.game_start;
    }

    // Method to change to gaming state
    public void change_to_gaming()
    {
        // Hide game start and game over UI
        if (this.game_obj_game_start != null)
            StartCoroutine(Canvas_grounp_fade.hide(this.game_obj_game_start));
        if (this.game_obj_game_over != null)
            StartCoroutine(Canvas_grounp_fade.hide(this.game_obj_game_over));

        // Destroy existing blocks
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        for (int i = 0; i < blocks.Length; i++)
        {
            Destroy(blocks[i]);
        }

        // Reset score
        this.num_score = -1;

        // Set last block
        this.game_obj_last_block = GameObject.FindGameObjectWithTag("block_0");

        // Create new block
        this.creat_new_block();

        // Change game status
        this.game_statu = Game_status.gaming;
    }

    // Method to change to game over state
    public void change_to_game_over()
    {
        // Hide game start UI and show game over UI
        if (this.game_obj_game_start != null)
            this.game_obj_game_start.SetActive(false);
        if (this.game_obj_game_over != null)
            StartCoroutine(Canvas_grounp_fade.show(this.game_obj_game_over));

        // Update and save record
        if (this.num_score > Config.record)
        {
            PlayerPrefs.SetInt("record", this.num_score);
            Config.record = this.num_score;
        }
        this.text_record.text = "Record" + ": " + Config.record;

        // Change game status
        this.game_statu = Game_status.game_over;

        // Activate blocks under the ground to allow them to be destroyed
        Destory_block_under_ground[] scripts = this.GetComponentsInChildren<Destory_block_under_ground>(true);
        for (int i = 0; i < scripts.Length; i++)
        {
            if (scripts[i].gameObject.activeSelf == false)
                scripts[i].gameObject.SetActive(true);
        }
    }

    // Method to restart the game
    public void change_to_game_again()
    {
        // Hide game start and game over UI
        StartCoroutine(Canvas_grounp_fade.hide(this.game_obj_game_start));
        StartCoroutine(Canvas_grounp_fade.hide(this.game_obj_game_over));

        // Create new block
        this.creat_new_block();

        // Change game status
        this.game_statu = Game_status.gaming;
    }

    // Event handlers
    #region 
    // Listener for start game button
    public void on_start_game_btn()
    {
        Main_control.instance.on_reset_btn();
    }

    // Listener for reset button
    public void on_reset_btn()
    {
        Main_control.instance.on_reset_btn();
    }
    #endregion
}

// Enumeration for game status
public enum Game_status
{
    game_start,
    gaming,
    game_over
}
