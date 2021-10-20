using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonedObject : MonoBehaviour
{
    public GameObject clone {get; set;}
    public GameObject reference {get; set;}
    public Vector3 offset {get; set;}
    public CloningScript cloningScript {get; set;}
    public CloningScript pairCloneScript {get; set;}
  
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
            clonePortal.linkedPortal = referencePortal.linkedPortal;
            if(reference.tag == "Portal"){
                clonePortal.setDummyObject(new GameObject(referencePortal.gameObject.name));
            }
            referencePortal.setClonePortal(clonePortal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateCloneRelations();
        switch(reference.tag){
            case "Player":
                PlayerHandle();
                break;
            case "Portal":
                FindWindow();
                PortalHandle();
                break;
            case "Window":
                WindowHandle();
                break;
            default:
                break;
        }
    }

    public void updateCloneRelations(){
        if(reference.tag == "Portal"){
            List<ClonedObject> listOfClonedPortals = cloningScript.getPairClonedPortalList();
            foreach(ClonedObject portal in listOfClonedPortals){
                if(portal.getReferencePortal().getPortalIdentifier() == referencePortal.linkedPortal.getPortalIdentifier()){
                    clonePortal.linkedPortal = portal.getClonePortal();
                    portal.getClonePortal().linkedPortal = clonePortal;
                }
            }
        }
    }

    public Portal getClonePortal(){
        return clonePortal;
    }

    public Portal getReferencePortal(){
        return referencePortal;
    }

    void PlayerHandle(){
        //print("pos " + clone.transform.position);
    }
    
    void PortalHandle(){
        //clonePortal.setViewTexture( referencePortal.linkedPortal.getViewTexture() );
        clonePortal.Render(null);
    }

    void WindowHandle(){
        clonePortal.Render(null);
    }

    void FindWindow(){
        List<ClonedObject> listOfClonedPortals = cloningScript.getClonedPortalList();
        foreach(ClonedObject portal in listOfClonedPortals){
            if(portal.getClonePortal().tag == "Window"){
                clonePortal.linkedPortal.setPlayerCam(portal.getClonePortal().getPortalCam());
                //clonePortal.setPlayerCam(portal.getClonePortal().getPortalCam());
            }
        }
    }
}
