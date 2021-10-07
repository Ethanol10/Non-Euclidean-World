using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonedObject : MonoBehaviour
{
    public GameObject clone {get; set;}
    public GameObject reference {get; set;}
    public Vector3 offset {get; set;}
    
    private Portal clonePortal;
    private Portal referencePortal;

    // Start is called before the first frame update
    void Start()
    {
        if(reference.tag == "Player"){
            //for now do nothing
        }
        if(reference.tag == "Portal" || reference.tag == "Window"){
            clonePortal = clone.GetComponent<Portal>();
            referencePortal = reference.GetComponent<Portal>();
            referencePortal.setClonedState();
            clonePortal.setPlayerCam(Camera.main);
            clonePortal.linkedPortal = referencePortal.linkedPortal;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(reference.tag){
            case "Player":
                PlayerHandle();
                break;
            case "Portal":
                PortalHandle();
                break;
            case "Window":
                WindowHandle();
                break;
            default:
                break;
        }
    }

    void PlayerHandle(){
        //print("pos " + clone.transform.position);
    }
    
    void PortalHandle(){
        clonePortal.CloneRender();
    }

    void WindowHandle(){
        clonePortal.Render(null);
    }
}
