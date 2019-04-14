using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotatingLevelManager : MonoBehaviour {

    [SerializeField]
    public List<LevelArea> areas;
    public LevelArea startArea;

    private int currentAreaIndex;
    private LevelArea currentArea;

    private Transform player;
    private Vector3 position;

    // Use this for initialization
    void Start() {
        //Error Checking
        if( startArea == null )
            Debug.LogError("LevelManager: Start Area Null");

        if( !areas.Contains(startArea) )
            Debug.LogError("LevelManager: Start Area not listed in areas");

        //Variable Initialization
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Initialize the positions
        currentAreaIndex = areas.IndexOf(startArea);
        currentArea = startArea;
        position = new Vector3(0, 0, 0);
        startArea.transform.localPosition = position;


        //Panels to the right
        position.x = currentArea.getWidth() / 2.0f;
        for( int i = currentAreaIndex + 1; i < areas.Count; i++ ) {
            position.x += areas[i].getWidth() / 2.0f;
            areas[i].transform.localPosition = position;
            position.x += areas[i].getWidth() / 2.0f;
        }

        //Panels to the left
        position.x = 0 - currentArea.getWidth() / 2.0f;
        for( int i = currentAreaIndex - 1; i >= 0; i-- ) {
            position.x -= areas[i].getWidth() / 2.0f;
            areas[i].transform.localPosition = position;
            position.x -= areas[i].getWidth() / 2.0f;
            //Debug.Log("Index: " + i + "; Width: " + areas[i].getWidth());
        }

        //SetupCamera();
    }

    // Update is called once per frame
    void Update() {
        //Check if Swapped area
        if( player.transform.position.x > currentArea.rightExtent() ) {
            //Check if need to loop
            if( currentAreaIndex == areas.Count - 2 ) {
                //Move the first area to the last spot in list
                areas.Add(areas[0]);
                areas.RemoveAt(0);

                //Update currentArea reference
                currentArea = areas[currentAreaIndex];

                //Move the area in World Space
                position.x = currentArea.rightExtent() + ( areas[areas.Count - 1].getWidth() / 2.0f );
                areas[areas.Count - 1].transform.localPosition = position;
            }
            else {
                //Update current area
                currentAreaIndex++;
                currentArea = areas[currentAreaIndex];
            }
        }
        else if( player.transform.position.x < currentArea.leftExtent() ) {
            //Check if need to loop
            if( currentAreaIndex == 1 ) {
                //Move the first area to the last spot in list
                areas.Insert(0, areas[areas.Count - 1]);
                areas.RemoveAt(areas.Count -1);

                //Update currentArea reference
                currentArea = areas[currentAreaIndex];

                //Move the area in World Space
                position.x = currentArea.leftExtent() - ( areas[0].getWidth() / 2.0f );
                areas[0].transform.localPosition = position;
            }
            else {
                //Update current area
                currentAreaIndex--;
                currentArea = areas[currentAreaIndex];
            }
        }
    }

    private void SetupCamera() {
        //Camera.main.rect = new Rect(0, startArea.transform.position.x + ( startArea.getHeight() / 2f ), startArea.getHeight() / 2f);
        Camera.main.orthographicSize = startArea.getHeight() / 2f;
    }
}
