using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMgr : MonoBehaviour
{
    //static public SceneMgr SceneMgrInstance;

    public void ChangeScene()
    {
        SceneManager.LoadScene("Scene_FirstLoading");
    }
}
