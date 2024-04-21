using UnityEngine;

public class Destory_block_under_ground : MonoBehaviour
{
    // Reference to the shadow plane
    private GameObject game_obj_shadow_plane;

    void Start()
    {
        // Find and store the shadow plane
        this.game_obj_shadow_plane = GameObject.FindGameObjectWithTag("shadow_plane");
    }

    void Update()
    {
        // If the game is in progress
        if (Stack_main_control.instance.game_statu == Game_status.gaming)
        {
            // Check if the block's y position is below the shadow plane
            if (this.transform.position.y < this.game_obj_shadow_plane.transform.position.y)
            {
                // If so, deactivate the block
                this.gameObject.SetActive(false);
            }
        }
    }
}
