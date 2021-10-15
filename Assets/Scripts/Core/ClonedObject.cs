using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonedObject : MonoBehaviour
{
    public GameObject clone {get; set;}
    public GameObject reference {get; set;}
    public Vector3 offset {get; set;}
    public CloningScript cloningScript {get; set;}
  
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
                PreClonePortalRender();
                PortalHandle();
                break;
            case "Window":
                PreClonePortalRender();
                WindowHandle();
                break;
            default:
                break;
        }
    }

    public Portal getClonePortal(){
        return clonePortal;
    }

    void PlayerHandle(){
        //print("pos " + clone.transform.position);
    }
    
    void PortalHandle(){
        clonePortal.setViewTexture( referencePortal.linkedPortal.getViewTexture() );
        clonePortal.CloneRender();
    }

    void WindowHandle(){
        clonePortal.Render(null);
    }

    //Render other clone portals before rendering the clone portal viewpoint.
    void PreClonePortalRender(){
        if( CameraUtility.VisibleFromCamera(referencePortal.screen, Camera.main)){
            Camera portalCamera = clonePortal.getPortalCam();
            List<ClonedObject> listOfClonedPortals = cloningScript.getClonedPortalList();
            foreach(ClonedObject portal in listOfClonedPortals){
                if( CameraUtility.VisibleFromCamera(portal.getClonePortal().screen, portalCamera) ){
                    print("I'm: " + portal.getClonePortal().linkedPortal.getPortalIdentifier());
                    portal.getClonePortal().linkedPortal.setPlayerCam(portalCamera);
                    portal.getClonePortal().Render(null);
                }
            }
        }
    }
}
