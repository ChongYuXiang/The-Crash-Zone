/* Author: Chong Yu Xiang  
 * Filename: Spin
 * Descriptions: Input values to spin an object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    // Input values
    public float rotateX;
    public float rotateY;
    public float rotateZ;

    // Update is called once per frame
    void Update()
    {
        // Rotate object over time
        transform.Rotate(rotateX * Time.deltaTime, rotateY * Time.deltaTime, rotateZ * Time.deltaTime, Space.Self);
    }
}
