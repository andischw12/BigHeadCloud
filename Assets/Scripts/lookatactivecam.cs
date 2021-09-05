using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class lookatactivecam : MonoBehaviour
{
   public Transform flow_target;
    
    private void LateUpdate()
    {
        if (Cameras.instance.ActiveCamera != null)
        {
            flow_target = Cameras.instance.ActiveCamera.transform;
            this.transform.LookAt(new Vector3(flow_target.transform.position.x, transform.position.y, flow_target.transform.position.z));
        }
    }
    
 
        
}
