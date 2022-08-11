using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class RemoveInactive : MonoBehaviour
{

    [SerializeField] GameObject Source;
    [SerializeField]  MeshRenderer[] myMeshes;
    [SerializeField]  MeshRenderer[] sourceMeshes;

    // Start is called before the first frame update
    void Start()
    {
          myMeshes = GetComponentsInChildren<MeshRenderer>();
          sourceMeshes = Source.GetComponentsInChildren<MeshRenderer>();
        {
            for(int i = 0; i < myMeshes.Length; i++) 
            {
                foreach (MeshRenderer m in sourceMeshes) 
                {
                    if (myMeshes[i].gameObject.name == m.gameObject.name) 
                    {
                        myMeshes[i].materials = m.materials;
                        continue;
                    }
                }
               
            }
        }
    }

    void RemoveInActive() 
    {
        Transform[] all = GetComponentsInChildren<Transform>(true);
        foreach (Transform t in all)
        {
            if (!t.gameObject.activeInHierarchy)
            {
                DestroyImmediate(t.gameObject);
            }
        }
    }

    
}
