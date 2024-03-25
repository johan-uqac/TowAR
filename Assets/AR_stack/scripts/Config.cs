using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Config : MonoBehaviour
{
    //version code
    public static string version_code = "v1.0.3";

    //days
    public static int lock_days = 7;

    //record
    public static int record = 0;

    //statu
    public static AR_statu ar_statu = AR_statu.recognizing;

    //远程配置的变量
    private struct userAttributes { }
    private struct appAttributes { }

    void Awake()
    {
        //set record
        #region 
        if (PlayerPrefs.GetInt("record") == 0)
        {
            Config.record =0;
        }
        else
        {
            Config.record =  PlayerPrefs.GetInt("record");
        }
        #endregion
    }

}

//AR识别时的状态
public enum AR_statu
{
    recognizing,                           //recognizing
    object_is_placed                       //object_is_placed
}