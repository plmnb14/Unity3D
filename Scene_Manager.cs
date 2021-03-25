using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public enum SceneName { Title, Lobby, StageSelect, BattleStage }

    public static Scene_Manager instance
    {
        get
        {
            if(null == m_instance)
            {
                m_instance = FindObjectOfType<Scene_Manager>();
            }

            return m_instance;
        }
    }

    private static Scene_Manager m_instance;

    private void Awake()
    {
        if(this != instance)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    static public void SceneChange(int sceneNumber)
    {
        string SceneName =
                sceneNumber == 0 ? "Scene_Title" :
                sceneNumber == 1 ? "Scene_Lobby" :
                sceneNumber == 2 ? "Scene_StageSelect" :
                sceneNumber == 3 ? "Scene_ShopMain" : "Scene_BattleStage";

        SceneManager.LoadScene(SceneName);
        // 이거 빌드 인덱스 순서로도 불러와지니 다음에 다시 한번 봐보자
    }
}
