using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Renderer))]
public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other){
        //when the trigger is hit by something
        //check if it is the projectile
        Projectile proj = other.GetComponent<Projectile>();

        if(proj != null){
            //if so, set goalMet to true
            Goal.goalMet = true;
            //set the alpha of the color to a higher opacity
            Material mat = GetComponent<Renderer>().material;

            Color c = mat.color;
            c.a = 0.75f;
            mat.color = c;
        }
    }

}
