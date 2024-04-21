using UnityEngine;

public class Destory_self : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Destroy this game object after 5 seconds
        GameObject.Destroy(this.gameObject, 5f);
    }
}
