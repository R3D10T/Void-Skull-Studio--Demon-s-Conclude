using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialTextManager : MonoBehaviour
{
    public GameObject[] Tutorials;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit)
        {
            if (hit.transform.tag == "Target")
            {
                Tutorials[1].SetActive(true);
            }

            if (hit.transform.tag == "Action")
            {
                Tutorials[0].SetActive(true);
                Tutorials[4].SetActive(true);
            }

            if (hit.transform.tag == "PlayerHealth")
            {
                Tutorials[2].SetActive(true);
            }

            if (hit.transform.tag == "EnemyHealth")
            {
                Tutorials[3].SetActive(true);
            }
        }
    }
}
