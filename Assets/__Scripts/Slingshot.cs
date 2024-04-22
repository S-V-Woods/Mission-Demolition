using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    
    [Header ("Inscribed")]//fields set in Unity Inspector pane
    public GameObject           projectilePrefab;
    public float velocityMult = 10f;
    public GameObject           projLinePrefab;
    
    [Header("Dynamic")]// fields set dynamically
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");//searches for child named LaunchPoint in the slingshot
        launchPoint = launchPointTrans.gameObject; //assigns GameObject to launchpoint field
        launchPoint.SetActive(false); //should the game ignore this?
        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter()
    {
        //print("Slingshot: OnMouseEnter()");
        launchPoint.SetActive(true); //will not ignore this and will use in Update or OnCollisonEnter. for most compoents just use enabled

    }

    void OnMouseExit()
    {
        //print("Slingshot: OnMouseExit()");
        launchPoint.SetActive(false);
        
    }
    void OnMouseDown()
    {
        //player has pressed the mouse down over the slingshot
        aimingMode = true;
        projectile = Instantiate(projectilePrefab) as GameObject;
        projectile.transform.position = launchPos; //set projectile at launchpoint
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
    void Update()
    {
        if(!aimingMode) return; // if not aiming then do nothing
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius; //limit to the radius of the slingshot collider
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *=maxMagnitude;
            Vector3 projPos = launchPos + mouseDelta;
            projectile.transform.position = projPos; //move projectile to the new positon
            if (Input.GetMouseButtonUp(0))
            {
                aimingMode = false; //mouse has been released
                Rigidbody projRB = projectile.GetComponent<Rigidbody>();
                projRB.isKinematic = false;              
                projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;  
                projRB.velocity = -mouseDelta *velocityMult;

                //FollowCam.SWITCH_VIEW(FollowCam.eView.both);
                FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);

                FollowCam.POI = projectile; // set the MainCamera POI
                //add a projectile to the projectile
                Instantiate<GameObject>(projLinePrefab, projectile.transform);
                projectile = null;
                MissionDemolition.SHOT_FIRED(); //since it is static it can be accessed

            }
        }
    }
}
