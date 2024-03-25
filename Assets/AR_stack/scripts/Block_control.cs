using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_control : MonoBehaviour
{
    private float move_speed = 0.5f;

    private Move_direction move_direction = Move_direction.direction_z;

    private bool is_moving = false;

    private GameObject ar_object;

    void Start()
    {
        this.ar_object = GameObject.FindGameObjectWithTag("ar_object");

        this.move_speed = Random.Range(0.25f, 0.3f);

        if (Stack_main_control.instance.game_obj_last_block.GetComponent<Block_control>() != null)
        {
            if (Stack_main_control.instance.game_obj_last_block.GetComponent<Block_control>().move_direction == Move_direction.direction_z)
            {
                this.move_direction = Move_direction.direction_x;
            }
            else
            {
                this.move_direction = Move_direction.direction_z;
            }
        }


        int color_index = Stack_main_control.instance.num_score;
        while (color_index >= Stack_main_control.colors.Length)
        {
            color_index -= Stack_main_control.colors.Length;
        }
        Color nowColor;
        ColorUtility.TryParseHtmlString(Stack_main_control.colors[color_index], out nowColor);
        this.GetComponent<Renderer>().material.color = nowColor;

        this.transform.localScale = new Vector3(Stack_main_control.instance.game_obj_last_block.transform.localScale.x,
            Stack_main_control.instance.game_obj_last_block.transform.localScale.y, Stack_main_control.instance.game_obj_last_block.transform.localScale.z);

        if (this.move_direction == Move_direction.direction_z)
            this.transform.position = new Vector3(Stack_main_control.instance.game_obj_last_block.transform.position.x,
                Stack_main_control.instance.game_obj_last_block.transform.position.y + this.transform.localScale.y, Stack_main_control.instance.game_obj_last_block.transform.position.z);
        else
            this.transform.position = new Vector3(Stack_main_control.instance.game_obj_last_block.transform.position.x,
                Stack_main_control.instance.game_obj_last_block.transform.position.y + this.transform.localScale.y, Stack_main_control.instance.game_obj_last_block.transform.position.z);

        this.is_moving = true;
    }

    void Update()
    {
        if (Stack_main_control.instance.game_statu == Game_stau.gaming)
        {
            if (this.is_moving == false)
                return;

            if (this.move_direction == Move_direction.direction_z)
            {
                this.transform.position += transform.forward * Time.deltaTime * this.move_speed;

                if (this.transform.position.z > Stack_main_control.instance.game_obj_last_block.transform.position.z + 0.275f)
                    this.move_speed = -1 * Mathf.Abs(this.move_speed);
                if (this.transform.position.z < Stack_main_control.instance.game_obj_last_block.transform.position.z - 0.275f)
                    this.move_speed = Mathf.Abs(this.move_speed);

            }
            else
            {
                this.transform.position += transform.right * Time.deltaTime * this.move_speed;

                if (this.transform.position.x > Stack_main_control.instance.game_obj_last_block.transform.position.x + 0.275f)
                    this.move_speed = -1 * Mathf.Abs(this.move_speed);
                if (this.transform.position.x < Stack_main_control.instance.game_obj_last_block.transform.position.x - 0.275f)
                    this.move_speed = Mathf.Abs(this.move_speed);
            }

            if (Input.GetMouseButtonDown(0))
            {
                this.stack_the_block();
            }
        }
    }

    private void stack_the_block()
    {
        this.is_moving = false;

        if (this.move_direction == Move_direction.direction_z)
        {
            float space_z = this.transform.position.z - Stack_main_control.instance.game_obj_last_block.transform.position.z;

            if (Mathf.Abs(space_z) < 0.005f)
            {
                this.transform.position = new Vector3(Stack_main_control.instance.game_obj_last_block.transform.position.x, this.transform.position.y, Stack_main_control.instance.game_obj_last_block.transform.position.z);
            }
            else
            {
                float new_block_size_z = Stack_main_control.instance.game_obj_last_block.transform.localScale.z - Mathf.Abs(space_z);

                float falling_block_size_z = this.transform.localScale.z - new_block_size_z;

                if (new_block_size_z <= 0)
                {
                    this.gameObject.AddComponent<Rigidbody>();
                    Destroy(this.gameObject, 2f);

                    Stack_main_control.instance.change_to_game_over();

                    return;
                }

                float new_block_position_z = Stack_main_control.instance.game_obj_last_block.transform.position.z + (space_z / 2);

                this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, new_block_size_z);
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, new_block_position_z);

                #region 

                int direction = 1;
                if (space_z < 0)
                    direction = -1;
                float cudeEdge = this.transform.position.z + (new_block_size_z / 2f * direction);
                float falling_block_posiiton_z = cudeEdge + falling_block_size_z / 2f * direction;

                GameObject game_obj_falling_block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                game_obj_falling_block.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, falling_block_size_z);
                game_obj_falling_block.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, falling_block_posiiton_z);

                game_obj_falling_block.GetComponent<Renderer>().material= this.GetComponent<Renderer>().material;

                game_obj_falling_block.AddComponent<Rigidbody>();
                game_obj_falling_block.transform.parent = this.ar_object.transform;
                #endregion
            }
        }
        else
        {
            float space_x = this.transform.position.x - Stack_main_control.instance.game_obj_last_block.transform.position.x;


            if (Mathf.Abs(space_x) < 0.005f)
            {
                this.transform.position = new Vector3(Stack_main_control.instance.game_obj_last_block.transform.position.x, this.transform.position.y, Stack_main_control.instance.game_obj_last_block.transform.position.z);
            }
            else
            {

                float new_block_size_x = Stack_main_control.instance.game_obj_last_block.transform.localScale.x - Mathf.Abs(space_x);

                float falling_block_size_x = this.transform.localScale.x - new_block_size_x;

                if (new_block_size_x <= 0)
                {
                    this.gameObject.AddComponent<Rigidbody>();
                    Destroy(this.gameObject, 2f);

                    Stack_main_control.instance.change_to_game_over();
                    return;
                }

                float new_block_position_x = Stack_main_control.instance.game_obj_last_block.transform.position.x + (space_x / 2);

                this.transform.localScale = new Vector3(new_block_size_x, this.transform.localScale.y, this.transform.localScale.z);
                this.transform.position = new Vector3(new_block_position_x, this.transform.position.y, this.transform.position.z);

                #region 
                int direction = 1;
                if (space_x < 0)
                    direction = -1;
                float cudeEdge = this.transform.position.x + (new_block_size_x / 2f * direction);
                float falling_block_posiiton_x = cudeEdge + falling_block_size_x / 2f * direction;

                GameObject game_obj_falling_block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                game_obj_falling_block.transform.localScale = new Vector3(falling_block_size_x, this.transform.localScale.y, this.transform.localScale.z);
                game_obj_falling_block.transform.position = new Vector3(falling_block_posiiton_x, this.transform.position.y, this.transform.position.z);

                game_obj_falling_block.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;

                game_obj_falling_block.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;

                game_obj_falling_block.AddComponent<Rigidbody>();
                game_obj_falling_block.transform.parent = this.ar_object.transform;
                #endregion
            }
        }

        Stack_main_control.instance.game_obj_last_block = this.gameObject;

        Stack_main_control.instance.creat_new_block();

        Destroy(this);
    }
}

public enum Move_direction
{
    direction_z,
    direction_x
}
