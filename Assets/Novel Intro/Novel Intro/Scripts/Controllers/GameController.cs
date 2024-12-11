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
            if (bottomBar.IsCompleted())
            {
                if (bottomBar.IsLastSentence())
                {
                    HandleSceneTransition();
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
            }
        }
    }

    void HandleSceneTransition()
    {
        string unitySceneName = currentScene.UnitySceneName;

        if (!string.IsNullOrEmpty(unitySceneName))
        {
            // Load the Unity scene specified in the current StoryScene
            SceneManager.LoadScene(unitySceneName);
        }
        else if (currentScene.nextScene != null)
        {
            // Otherwise, proceed to the next in-game StoryScene
            currentScene = currentScene.nextScene;
            bottomBar.PlayScene(currentScene);
            backgroundController.SwitchImage(currentScene.background);
        }
        else
        {
            Debug.LogWarning("No next scene or Unity scene specified!");
        }
    }
}
