using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonTeens
{
    public class DESTROYER : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}