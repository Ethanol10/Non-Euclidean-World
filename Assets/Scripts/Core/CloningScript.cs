using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloningScript : MonoBehaviour
{   
    //Spawning Templates
    public GameObject samplePortal;
    public GameObject samplePlayer;
    public GameObject sampleWindow;

    //Custom offset
    public Vector3 offset = new Vector3(500, 500, 500);

    //Parent of all objects within the collider.
    private GameObject clonedScene;
    private GameObject objectContainer;
    private List<ClonedObject> cloneObjectList;
    private MeshRenderer cloningBoxMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Make debug box invisible.
        cloningBoxMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        cloningBoxMeshRenderer.enabled = false;

        //Put the cloned scene into a gameobject and make it the parent.
        clonedScene = new GameObject("Cloned Scene");
        clonedScene.transform.parent = gameObject.transform;
        objectContainer = new GameObject("Cloned Scene object list");
        objectContainer.transform.parent = gameObject.transform;

        //Instantiate clonedobject list.
        cloneObjectList = new List<ClonedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collidedObject){
        bool found = false;

        foreach(ClonedObject cloneOb in cloneObjectList){
            if( (cloneOb.reference.GetInstanceID() == collidedObject.gameObject.GetInstanceID())
                    || (cloneOb.clone.GetInstanceID() == collidedObject.gameObject.GetInstanceID()) ){
                found = true;
            }
        }

        //We don't want to clone portal frames.
        if(!found && (collidedObject.gameObject.tag != "PortalFrame")){
            //Container for object
            GameObject container = new GameObject(collidedObject.gameObject.name);
            container.transform.parent = objectContainer.transform;

            //Creating component on container.
            ClonedObject newClone = container.AddComponent<ClonedObject>();
            newClone.reference = collidedObject.gameObject;

            //Edge Case Handling: Don't want to disrupt the portal cameras, so we need to make sure that
            //a new object similar to it has been created.
            switch(newClone.reference.tag){
                case "Portal":
                    newClone.clone = Instantiate(samplePortal, clonedScene.transform);
                    break;
                case "Player":
                    newClone.clone = Instantiate(samplePlayer, clonedScene.transform);
                    break;
                case "Window":
                    newClone.clone = Instantiate(sampleWindow, clonedScene.transform);
                    break;
                default:
                    newClone.clone = Instantiate(newClone.reference, clonedScene.transform);
                    break;
            }

            newClone.clone.transform.position = newClone.reference.transform.position + offset;
            newClone.clone.transform.localScale = newClone.reference.transform.localScale;
            newClone.clone.transform.rotation = newClone.reference.transform.rotation;
            newClone.offset = offset;
            cloneObjectList.Add(newClone);
        }
    }
    
    void OnTriggerExit(Collider exitingObject){
        for(int i = 0; i < cloneObjectList.Count; i++){
            if(exitingObject.GetInstanceID() == cloneObjectList[i].reference.GetInstanceID()){
                Destroy(cloneObjectList[i].clone);
                cloneObjectList.RemoveAt(i);
                i = cloneObjectList.Count;
            }
        }
    }
}
