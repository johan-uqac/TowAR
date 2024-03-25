using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_object_control : MonoBehaviour
{
    public GameObject stack_parent;

    private GameObject game_obj_block_0;

    private Vector3 init_position;

    void Start()
    {
        this.game_obj_block_0 = GameObject.FindGameObjectWithTag("block_0");

        this.init_position = this.stack_parent.transform.position;

        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        if (Stack_main_control.instance.game_statu == Game_stau.gaming)
        {
            float height = (this.game_obj_block_0.transform.localScale.y-0.01f) * (Stack_main_control.instance.num_score -10);
            if (height < 0)
                height = 0;

            this.stack_parent.transform.position = Vector3.Lerp(this.stack_parent.transform.position, new Vector3(this.init_position.x, this.init_position.y - height, this.init_position.z), Time.deltaTime * 2.5f);
        }
        else if (Stack_main_control.instance.game_statu == Game_stau.game_over)
        {
            this.stack_parent.transform.position = Vector3.Lerp(this.stack_parent.transform.position, this.init_position, Time.deltaTime * 2.5f);
        }
    }
}
