using UnityEngine;

public class RotateCtrl : MonoBehaviour
{
    // Direction of rotation
    public Vector3 dir = new Vector3(0, 0, 1);

    // Flag to control rotation
    private bool isStop;

    // Update is called once per frame
    void Update()
    {
        // Check if rotation is not stopped
        if (!isStop)
        {
            // Rotate the object continuously
            this.transform.Rotate(dir * Time.deltaTime * 100f, Space.Self);
        }
    }

    // Method to stop rotation
    public void StopRotate()
    {
        isStop = true;
    }

    // Method called when the object is enabled
    private void OnEnable()
    {
        // Ensure rotation is not stopped when object is enabled
        isStop = false;
    }
}
