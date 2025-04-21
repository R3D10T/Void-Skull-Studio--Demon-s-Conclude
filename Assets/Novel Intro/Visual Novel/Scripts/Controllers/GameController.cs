using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public StoryScene currentScene;
    public BottomBarController bottomBar;
    public BackgroundController backgroundController;

    public GameObject cam1;
    public GameObject cam2;

    void Start()
    {
        bottomBar.PlayScene(currentScene);
        backgroundController.SetImage(currentScene.background);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (!bottomBar.IsCompleted())
            {
                bottomBar.SkipText();
            }
            else if (bottomBar.IsLastSentence())
            {
                if (currentScene.nextScene == null)
                {
                    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                    int nextSceneIndex = currentSceneIndex + 1;

                    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                    {
                        SceneManager.LoadScene(nextSceneIndex);
                    }
                }

                currentScene = currentScene.nextScene;

                if (currentScene != null)
                {
                    if (GetSceneName(currentScene) == "4")
                    {
                        if (cam1 != null) cam1.SetActive(true);
                        if (cam2 != null) cam2.SetActive(false);
                    }

                    bottomBar.PlayScene(currentScene);
                    backgroundController.SwitchImage(currentScene.background);
                }
            }
            else
            {
                bottomBar.PlayNextSentence();
            }
        }
    }

    string GetSceneName(StoryScene scene)
    {
        return scene != null ? scene.name : "None";
    }
}
