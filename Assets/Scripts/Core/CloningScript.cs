using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloningScript : MonoBehaviour
{   
    //Spawning Templates
    public GameObject samplePortal;
    public GameObject samplePlayer;
    public GameObject sampleWindow;
    public MainCamera MainCamera;
    public Portal targetPortal;

    //Custom offset
    public Vector3 offset = new Vector3(500, 500, 500);
    private Vector3 scaler;
    //ScalerY should only apply to the child object window.
    private float scalerY;

    //Parent of all objects within the collider.
    private Vector3 originalScale;
    private GameObject clonedScene;
    private GameObject objectContainer;
    private List<ClonedObject> cloneObjectList;
    private MeshRenderer cloningBoxMeshRenderer;
    private List<ClonedObject> clonedPortalList;

    // Awake is called before the first frame update
    //Ensure scaler is set before the first frame update to prevent the scaler from
    //not working as it should
    void Awake(){
        scaler = new Vector3(1.0f, 1.0f, targetPortal.gameObject.transform.localScale.x);
        scalerY = targetPortal.gameObject.transform.localScale.y;
        print(scaler);
    }

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
        originalScale = clonedScene.transform.localScale;
        print("originalScale " + gameObject.name + ":" + originalScale);

        //Instantiate clonedobject list.
        cloneObjectList = new List<ClonedObject>();
        clonedPortalList = new List<ClonedObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<ClonedObject> getClonedPortalList(){
        return clonedPortalList;
    }

    public void setScaler(Vector3 newScaler){
        scaler = newScaler;
    }

    void OnTriggerEnter(Collider collidedObject){
        bool found = false;
        //print("onTrigger Scaler: " + scaler);

        foreach(ClonedObject cloneOb in cloneObjectList){
            if( (cloneOb.reference.GetInstanceID() == collidedObject.gameObject.GetInstanceID())
                    || (cloneOb.clone.GetInstanceID() == collidedObject.gameObject.GetInstanceID()) ){
                found = true;
            }
        }

        //We don't want to clone portal frames.
        if(!found && (collidedObject.gameObject.tag != "PortalFrame")){
            ScaleToOriginal();
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
                    newClone.clone = Instantiate(samplePlayer);
                    break;
                case "Window":
                    newClone.clone = Instantiate(sampleWindow, clonedScene.transform);
                    newClone.clone.transform.localScale = new Vector3(1.0f, scalerY, 1.0f);
                    break;
                default:
                    newClone.clone = Instantiate(newClone.reference, clonedScene.transform);
                    break;
            }

            if(newClone.reference.tag != "Window"){
                newClone.clone.transform.localScale = newClone.reference.transform.localScale;
            }
            newClone.clone.transform.rotation = newClone.reference.transform.rotation;
            newClone.clone.transform.position = newClone.reference.transform.position + offset;
            newClone.offset = offset;
            cloneObjectList.Add(newClone);
            //Add to list of cloned portals to ensure that cloned objects can refer to an
            //updated list at all times.
            if(newClone.reference.tag == "Portal" || newClone.reference.tag == "Window"){
                clonedPortalList.Add(newClone);
            }
            newClone.cloningScript = gameObject.GetComponent<CloningScript>();
            ScaleBack();
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

    void ScaleToOriginal(){
        clonedScene.transform.localScale = originalScale;
    }

    void ScaleBack(){
        print("ClonedScene LocalScale: " + gameObject.name + " :" + clonedScene.transform.localScale);
        clonedScene.transform.localScale = Vector3.Scale(clonedScene.transform.localScale, scaler);
    }
}
