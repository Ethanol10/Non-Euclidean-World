using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonedObject : MonoBehaviour
{
    public GameObject clone {get; set;}
    public GameObject reference {get; set;}
    public Vector3 offset {get; set;}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(reference.tag == "Player"){
            clone.transform.position = reference.transform.position + offset;
            clone.transform.rotation = reference.transform.rotation;
        }

    }
}
