using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    List<Portal> portals;
    GameObject player;

    void Awake () {
        portals = new List<Portal>();
        resetPortalIdentification();
        // portals = FindObjectsOfType<Portal>();
        GameObject[] returnedPlayerArray;
        returnedPlayerArray = GameObject.FindGameObjectsWithTag("Player");
        player = returnedPlayerArray[0];
    }

    void OnPreCull () {
        List<Portal> portalsList = new List<Portal>();
        List<float> portalDistances = new List<float>();
        portalsList.Clear();
        portalDistances.Clear();
        
        //Pre-render
        for (int i = 0; i < portals.Count; i++) {
            portals[i].PrePortalRender ();
        }

        //Need to sort by distance from player.
        for (int i = 0; i < portals.Count; i++) {
            Portal portal = portals[i];
            portalDistances.Add(Vector3.Distance(player.transform.position, portal.transform.position));
            portalsList.Add(portal);
            // portals[i].Render (portals, player);
        }
        
        //Sort to figure out which distance is smaller and put that at the back of the list to render.
        for (int i = 0; i < portalDistances.Count - 1; i++){
            for (int j = 0; j < portalDistances.Count - i - 1; j++){
                if (portalDistances[j] < portalDistances[j + 1]){
                    float temp = portalDistances[j];    
                    Portal transformTemp = portalsList[j];

                    portalDistances[j] = portalDistances[j + 1];
                    portalsList[j] = portalsList[j + 1];

                    portalDistances[j + 1] = temp;
                    portalsList[j + 1] = transformTemp;
                }
            }
        }

        //Check the visibility of the portal from the main camera.
        for (int i = 0; i < portalsList.Count; i++) {
            portalsList[i].checkVisibility();
        }

        //render portal
        for (int i = 0; i < portalsList.Count; i++) {
            portalsList[i].Render(portalsList);
        }

        //Post Render
        for (int i = 0; i < portals.Count; i++) {
            portals[i].PostPortalRender ();
        }
    }

    public void resetPortalIdentification(){
        Portal[] portalArr;
        portals.Clear();    
        portalArr = FindObjectsOfType<Portal>();
        for(int i = 0; i < portalArr.Length; i++){
            portals.Add(portalArr[i]);
        }
    }
} 