using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]
public class StoryScene : ScriptableObject
{
    public List<Sentence> sentences;
    public Sprite background;
    public StoryScene nextScene;

    [SerializeField]
    private SceneAsset nextUnityScene;

    public string UnitySceneName
    {

        get
        {
            // Return the name of the scene asset if it's set
            return nextUnityScene != null ? nextUnityScene.name : string.Empty;
        }
    }

    [System.Serializable]
    public struct Sentence
    {
        public string text;
        public Speaker speaker;
    }
}
