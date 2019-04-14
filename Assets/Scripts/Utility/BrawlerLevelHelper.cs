using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerLevelHelper : MonoBehaviour {

    public static BrawlerLevelHelper Instance {
        get { return s_Instance; }
    }

    protected static BrawlerLevelHelper s_Instance;

    public float layerStepSize = 1f;
    public float yOffset = 0f;
    public float scale = 1f;

    public bool scalingEnabled;

    void Awake() {
        if( s_Instance == null )
            s_Instance = this;
        else
            throw new UnityException("There cannot be more than one BrawlerLevelHelper script.  The instances are " + s_Instance.name + " and " + name + ".");
    }

    void OnEnable() {
        if( s_Instance == null )
            s_Instance = this;
        else if( s_Instance != this )
            throw new UnityException("There cannot be more than one BrawlerLevelHelper script.  The instances are " + s_Instance.name + " and " + name + ".");
    }

    void OnDisable() {
        s_Instance = null;
    }

    static public int getTranslatedLayer(float yPos) {
        return -(int)( ( yPos - Instance.yOffset ) / Instance.layerStepSize );
    }
}
