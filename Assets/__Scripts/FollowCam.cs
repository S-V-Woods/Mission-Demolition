using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static private FollowCam S; // another private singleton

    static public GameObject POI; // static point of interest. all instances of follow cam can assess it
    //access as FollowCam.POI. this allows slingshot to know which projectile to follow
    
    public enum eView { none, slingshot, castle, both};

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero; //Vector2.zero is [0,0]
    public GameObject viewBothGO;


    [Header ("Dynamic")]
    public float camZ; //the desired Z pos of the camera
    public eView nextView = eView.slingshot;

    void Awake()
    {
        S = this;
        camZ = this.transform.position.z; 
    }

    public void SwitchView( eView newView){
        if(newView == eView.none){
            newView = nextView;
        }
        switch (newView){
            case eView.slingshot:
                POI = null;
                nextView = eView.castle;
                break;

            case eView.castle:
                POI = MissionDemolition.GET_CASTLE();
                nextView = eView.both;
                break;
            
            case eView.both:
                POI = viewBothGO;
                nextView = eView.slingshot;
                break;
        }
    }

    public void SwitchView(){
        SwitchView(eView.none);
    }

    static public void SWITCH_VIEW (eView newView){
        S.SwitchView(newView);
    }

    void FixedUpdate() //use FixedUpdate
    {
        /*
        if (POI == null) return; //without a projectile then null
        Vector3 destination = POI.transform.position; //get the position of the POI
        */
        Vector3 destination = Vector3.zero;
        if( POI != null)
        {
            // if there is Rigidbody check if sleeping
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            // no rigidbody then return null
            if((poiRigid !=null) && poiRigid.IsSleeping()) //no need to simulation anymore, bud
            {
                POI = null;
            }
        }
        if ( POI != null)
        {
            destination = POI.transform.position;
        }
        //limit the mix and max values so they don't go passed the boundaries
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //interpolate from current camera position toward destination
        //Lerp is linear interpelation. 0 is the first vector and 1 is the second vector

        
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z  = camZ; // keeps the camera far away enough
        transform.position = destination;
        //set orthographicSize of the camera to 10 since the ground is there and it'll stay in view. using maz() means that it'll never be under 0
        Camera.main.orthographicSize = destination.y + 10;
    }


}
