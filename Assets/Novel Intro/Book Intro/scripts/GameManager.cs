using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _StartingSceneTransition;
    [SerializeField] private GameObject _EndingSceneTransition;
    [SerializeField] private Book book; // Reference to the Book script

    private void Start()
    {
        _StartingSceneTransition.SetActive(true);
        Invoke(nameof(DisableStartingSceneTransition), 5f);
    }

    private void DisableStartingSceneTransition()
    {
        _StartingSceneTransition.SetActive(false);
    }

    private void Update()
    {
        // Check the currentPage from the Book script
        if (book.currentPage >= 2)
        {
            StartCoroutine(TransitionScene());
        }
    }

    private IEnumerator TransitionScene()
    {
        if (_EndingSceneTransition != null)
        {
            _EndingSceneTransition.SetActive(true);
        }

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
