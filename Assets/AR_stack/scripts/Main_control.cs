using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using System;
using UnityEngine.SceneManagement;

public class Main_control : MonoBehaviour
{
    public static Main_control instance;

    public GameObject objectToPlace;

    public GameObject placementIndicator;

    public GameObject gameobject_place_btn;

    public GameObject gameobject_reset_btn;

    public GameObject gameobject_hint_scanning;

    public Text Text_debug;

    [Header("Directional light's transform")]
    public Transform transform_directional_light;


    private Pose placementPose;

    private bool placementPoseIsValid = false;

    private ARSessionOrigin arOrigin;

    private ARRaycastManager raycastManager;

    void Awake()
    {
        Main_control.instance = this;
    }

    void Start()
    {
        this.arOrigin = FindObjectOfType<ARSessionOrigin>();

        this.raycastManager = FindObjectOfType<ARRaycastManager>();

        this.change_to_recognizing();
    }

    void Update()
    {
        if (Config.ar_statu == AR_statu.recognizing)
        {
            this.UpdatePlacementPose();

            this.UpdatePlacementIndicator();
        }
    }

    public void change_to_recognizing()
    {
        Config.ar_statu = AR_statu.recognizing;

        GameObject[] objs = GameObject.FindGameObjectsWithTag("ar_object");
        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }

        this.gameobject_place_btn.SetActive(false);

        this.gameobject_reset_btn.SetActive(false);
    }

    public void change_to_object_is_placed()
    {
        Config.ar_statu = AR_statu.object_is_placed;

        this.PlaceObject();

        this.gameobject_reset_btn.SetActive(true);

        this.gameobject_place_btn.SetActive(false);

        this.placementIndicator.SetActive(false);
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        var hits = new List<ARRaycastHit>();

        this.raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);


        this.placementPoseIsValid = hits.Count > 0;


        if (this.placementPoseIsValid)
        {
            this.placementPose = hits[0].pose;

            float distance = (this.placementPose.position - Camera.main.transform.position).sqrMagnitude;

            if (distance < 0.15f || distance > 5.5f)
                this.placementPoseIsValid = false;
            else
            {
                var cameraForward = Camera.main.transform.forward;
                var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

                this.placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        }
    }

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

    private void PlaceObject()
    {
        GameObject obj= Instantiate(this.objectToPlace, this.placementPose.position, this.placementPose.rotation);

        this.transform_directional_light.position = Camera.main.transform.position;
        this.transform.LookAt(obj.transform.position);

    }

    public void switch_buidling()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ar_object");
        for (int i = 0; i < objs.Length; i++)
        {
            Destroy(objs[i].gameObject);
        }

        this.PlaceObject();
    }

    //event
    #region 
    //监听重置按钮 Monitor reset button
    public void on_reset_btn()
    {
        this.change_to_recognizing();
    }

    //监听返回按钮 Monitor back button
    public void on_back_btn()
    {
        SceneManager.LoadSceneAsync("main_ui");
    }

    public void on_place_btn()
    {
        this.change_to_object_is_placed();
    }
    #endregion
}

