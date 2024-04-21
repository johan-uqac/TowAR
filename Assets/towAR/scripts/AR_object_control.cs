using UnityEngine;

public class AR_object_control : MonoBehaviour
{
    // Reference to the parent object that holds the stack of blocks
    public GameObject stack_parent;

    // Reference to the initial block (block_0) in the stack
    private GameObject game_obj_block_0;

    // Initial position of the stack_parent
    private Vector3 init_position;

    void Start()
    {
        // Find and store the initial block (block_0)
        this.game_obj_block_0 = GameObject.FindGameObjectWithTag("block_0");

        // Store the initial position of the stack_parent
        this.init_position = this.stack_parent.transform.position;

        // Reset the rotation of the AR object to zero
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        // If the game is in progress
        if (Stack_main_control.instance.game_statu == Game_status.gaming)
        {
            // Calculate the height based on the score and adjust the stack's position
            float height = (this.game_obj_block_0.transform.localScale.y - 0.01f) * (Stack_main_control.instance.num_score - 10);
            if (height < 0)
                height = 0;

            // Smoothly move the stack_parent to the adjusted position
            this.stack_parent.transform.position = Vector3.Lerp(this.stack_parent.transform.position, new Vector3(this.init_position.x, this.init_position.y - height, this.init_position.z), Time.deltaTime * 2.5f);
        }
        // If the game is over
        else if (Stack_main_control.instance.game_statu == Game_status.game_over)
        {
            // Smoothly move the stack_parent back to its initial position
            this.stack_parent.transform.position = Vector3.Lerp(this.stack_parent.transform.position, this.init_position, Time.deltaTime * 2.5f);
        }
    }
}
