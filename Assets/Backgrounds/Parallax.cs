
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Parallax : MonoBehaviour
{

    private float length;
    private float StartPos;
    public GameObject Camera;
    public float speed;
    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (Camera.transform.position.x * (1 - speed));
        float distance = (Camera.transform.position.x * speed);

        transform.position = new Vector3(StartPos + distance, transform.position.y, transform.position.z);

        if (temp > StartPos + (length - offset))
        {
            StartPos += length;
        }
        else if (temp < StartPos - (length - offset))
        {
            StartPos -= length;
        }

    }
}