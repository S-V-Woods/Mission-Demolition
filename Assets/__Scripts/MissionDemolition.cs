using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // a private singleton

    [Header("Inscribed")]
    public Text         uitLevel;   // UIText_Level
    public Text         uitShots;   // UIText_Shots
    public Vector3      castlePos;  // The place to put the castles
    public GameObject[]   castles;    // Array of castles

    [Header("Dynamic")]
    public int          level;      // current level
    public int          levelMax;   //number of levels

    public int          shotsTaken;
    public GameObject   castle;     // current castle
    public GameMode     mode = GameMode.idle;
    public string       showing = "Show Slingshot"; //FollowCam mode



    // Start is called before the first frame update
    void Start()
    {
        S = this; //define singleton

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel(); //call startLevel method
    }

    void StartLevel()
    {
        //get rid of any castles that exists
        if (castle != null){
            Destroy(castle);
        }

    //Destroy old projectiles (method not written yet)
        Projectile.DESTROY_PROJECTILES();

        //instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        //reset the goal
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
        //Zoom out to show both
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void NextLevel(){
        level++;
        if(level == levelMax){
            level = 0;
            shotsTaken = 0;
        }
        StartLevel();
    }
        //static mothod that allows code anywhere to increment shotsTaken
    static public void SHOT_FIRED(){
        S.shotsTaken++;
    }

    //Static method that allows code anywhere to get a refence to S.castle
    static public GameObject GET_CASTLE(){
        return S.castle;
    }


    void Update(){
        UpdateGUI();
        
        // check for the level end
        if ((mode == GameMode.playing) && Goal.goalMet){
            //change mode to stop checking for the level end
            mode = GameMode.levelEnd;
            //zoom out to view both
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            //Start the next level in 2 seconds
            Invoke ("NextLevel", 2f);
        }
    }

    // Update is called once per frame
    void UpdateGUI()
    {
        //show the data in the GUITexts
        uitLevel.text = "Level: " +(level + 1) + " of "+ levelMax;
        uitShots.text = "Shots Taken: "+ shotsTaken;
        
    }
}



