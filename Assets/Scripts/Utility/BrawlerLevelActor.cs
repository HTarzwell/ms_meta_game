using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class BrawlerLevelActor : MonoBehaviour {

    protected static BrawlerLevelSettings settingsInstance;

    [System.Serializable]
    public class BrawlerLevelSettings {
        public int testVar = 10;
    }

    public BrawlerLevelSettings getInstance() {
        if( settingsInstance == null ) {
            TextAsset jsonObj = Resources.Load(Path.Combine("BrawlerJSON",SceneManager.GetActiveScene().name)) as TextAsset;
            if( jsonObj == null )
                settingsInstance = new BrawlerLevelSettings();
            else
                settingsInstance = JsonUtility.FromJson<BrawlerLevelSettings>(jsonObj.text);
        }
        return settingsInstance;
    }

    private void Awake() {
        if( settingsInstance == null )
            settingsInstance = getInstance();
    }

    void OnEnable() {
        if( settingsInstance == null )
            settingsInstance = getInstance();

    }

    private void OnDestroy() {

    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
