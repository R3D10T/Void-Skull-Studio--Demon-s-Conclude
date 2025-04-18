using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public StoryScene currentScene;
    public BottomBarController bottomBar;
    public BackgroundController backgroundController;

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
                bottomBar.PlayScene(currentScene);
                backgroundController.SwitchImage(currentScene.background);
            }
            else
            {
                bottomBar.PlayNextSentence();
            }
        }
    }
}
