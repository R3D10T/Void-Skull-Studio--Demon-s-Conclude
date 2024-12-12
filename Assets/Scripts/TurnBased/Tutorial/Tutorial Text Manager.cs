using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialTextManager : MonoBehaviour
{
    public GameObject[] Tutorials;
    public GameObject Filter;
    public bool isPaused;
    public bool hasTargetted;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Tutorials.Length; i++)
        {
            Tutorials[i].SetActive(false);
        }
        Filter.SetActive(false);
        isPaused = false;
        hasTargetted = false;

        actionstut();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void actionstut()
    {
        Filter.SetActive(true);
        Tutorials[0].SetActive(true);
        isPaused=true;
        Time.timeScale = 0f;
    }

    public void attacktut()
    {
        Filter.SetActive(true);
        Tutorials[4].SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void PHPtut()
    {
        Filter.SetActive(true);
        Tutorials[2].SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void EHPtut()
    {
        Filter.SetActive(true);
        Tutorials[3].SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void Targettut()
    {
        if (!hasTargetted)
        {
            Filter.SetActive(true);
            Tutorials[1].SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
            hasTargetted=true;
        }
    }

    public void resumeGame(GameObject tut)
    {
        if (isPaused)
        {
            isPaused = false;
            tut.SetActive(false);
            Filter.SetActive(false);
            Time.timeScale=1.0f;

            if (tut == Tutorials[0])
            {
                PHPtut();
            }
            if (tut == Tutorials[2])
            {
                EHPtut();
            }
            if (tut == Tutorials[3])
            {
                attacktut();
            }
        }

    }
}
