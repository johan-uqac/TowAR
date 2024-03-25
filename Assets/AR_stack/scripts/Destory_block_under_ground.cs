using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destory_block_under_ground : MonoBehaviour
{
    private GameObject game_obj_shadow_plane;

    void Start()
    {
        this.game_obj_shadow_plane = GameObject.FindGameObjectWithTag("shadow_plane");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.transform.position.y);
        if (Stack_main_control.instance.game_statu == Game_stau.gaming)
            if (this.transform.position.y < this.game_obj_shadow_plane.transform.position.y)
            {
                this.gameObject.SetActive(false);
                //Debug.Log(this.transform.position.y);
            }              
    }
}
