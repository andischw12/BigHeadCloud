using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundFlying : MonoBehaviour
{
      [SerializeField] GameObject target;
     [SerializeField] float angel;

    void FixedUpdate()
    {
        // Spin the object around the target at 20 degrees/second.
        transform.RotateAround(target.transform.position, Vector3.up, angel * Time.deltaTime);
    }
}
