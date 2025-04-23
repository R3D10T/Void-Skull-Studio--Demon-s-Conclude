using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject Overlay;

    public GameObject[] Tutorial;

    int CurTut = 0;

    bool tutOver = false;
    // Start is called before the first frame update
    void Start()
    {
        Overlay.SetActive(true);
        Tutorial[0].SetActive(true);

    }

    void Update()
    {
        if (tutOver == false) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                Tutorial[CurTut].SetActive(false);
                CurTut++;
                if (CurTut < Tutorial.Length)
                {
                    Tutorial[CurTut].SetActive(true);
                }
                else
                {
                    Overlay.SetActive(false);
                    tutOver = true;
                }
            }
        }
    }
}
