using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMoving : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public float speed;

    private bool toPos2 = true;

    // Update is called once per frame
    void Update()
    {
        if (toPos2)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos2.transform.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pos1.transform.position, speed * Time.deltaTime);
        }

        if (transform.position.x == pos2.transform.position.x)
        {
            toPos2 = false;
        }
        if (transform.position.x == pos1.transform.position.x)
        {
            toPos2 = true;
        }
    }
}
