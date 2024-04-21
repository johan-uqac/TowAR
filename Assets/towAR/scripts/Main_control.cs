using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class Main_control : MonoBehaviour
{
    // Singleton instance
    public static Main_control instance;

    // Prefab of the object to be placed
    public GameObject objectToPlace;

    // Indicator for object placement
    public GameObject placementIndicator;

    // Button to place object
    public GameObject gameobject_place_btn;

    // Button to reset placement
    public GameObject gameobject_reset_btn;

    // Hint for scanning
    public GameObject gameobject_hint_scanning;

    // Debug text
    public Text Text_debug;

    // Directional light's transform
    [Header("Directional light's transform")]
    public Transform transform_directional_light;

    // Pose for object placement
    private Pose placementPose;

    // Flag indicating whether placement pose is valid
    private bool placementPoseIsValid = false;

    // AR session origin
    private ARSessionOrigin arOrigin;

    // AR raycast manager
    private ARRaycastManager raycastManager;

    // Initialize singleton instance
    void Awake()
    {
        Main_control.instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find AR session origin and raycast manager
        this.arOrigin = FindObjectOfType<ARSessionOrigin>();
        this.raycastManager = FindObjectOfType<ARRaycastManager>();

        // Start in recognizing mode
        this.change_to_recognizing();
    }

    // Update is called once per frame
    void Update()
    {
        // Update placement pose and indicator in recognizing mode
        if (Config.ar_statu == AR_statu.recognizing)
        {
            this.UpdatePlacementPose();
            this.UpdatePlacementIndicator();
        }
    }

    // Switch to recognizing mode
    public void change_to_recognizing()
    {
        Config.ar_statu = AR_statu.recognizing;

        // Destroy placed objects
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ar_object");
        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }

        // Deactivate buttons
        this.gameobject_place_btn.SetActive(false);
        this.gameobject_reset_btn.SetActive(false);
    }

    // Switch to object placement mode
    public void change_to_object_is_placed()
    {
        Config.ar_statu = AR_statu.object_is_placed;

        // Place object
        this.PlaceObject();

        // Activate reset button and deactivate placement indicator
        this.gameobject_reset_btn.SetActive(true);
        this.gameobject_place_btn.SetActive(false);
        this.placementIndicator.SetActive(false);
    }

    // Update placement pose based on detected planes
    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        this.raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        this.placementPoseIsValid = hits.Count > 0;

        if (this.placementPoseIsValid)
        {
            this.placementPose = hits[0].pose;

            // Check distance for valid placement
            float distance = (this.placementPose.position - Camera.main.transform.position).sqrMagnitude;
            if (distance < 0.15f || distance > 5.5f)
                this.placementPoseIsValid = false;
            else
            {
                // Adjust rotation to face camera
                var cameraForward = Camera.main.transform.forward;
                var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                this.placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        }
    }

    // Update placement indicator based on placement pose validity
    private void UpdatePlacementIndicator()
    {
        if (this.placementPoseIsValid)
        {
            this.placementIndicator.SetActive(true);
            this.placementIndicator.transform.SetPositionAndRotation(this.placementPose.position, this.placementPose.rotation);
            StartCoroutine(Canvas_grounp_fade.hide(this.gameobject_hint_scanning));
            this.gameobject_place_btn.SetActive(true);
        }
        else
        {
            this.placementIndicator.SetActive(false);
            StartCoroutine(Canvas_grounp_fade.show(this.gameobject_hint_scanning));
            this.gameobject_place_btn.SetActive(false);
        }
    }

    // Place the object at the placement pose
    private void PlaceObject()
    {
        GameObject obj = Instantiate(this.objectToPlace, this.placementPose.position, this.placementPose.rotation);
        this.transform_directional_light.position = Camera.main.transform.position;
        this.transform.LookAt(obj.transform.position);
    }

    // Reset object placement
    public void switch_buidling()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ar_object");
        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }
        this.PlaceObject();
    }

    // Event listeners
    #region 
    // Reset button listener
    public void on_reset_btn()
    {
        this.change_to_recognizing();
    }

    // Place button listener
    public void on_place_btn()
    {
        this.change_to_object_is_placed();
    }
    #endregion
}
