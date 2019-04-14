using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArea : MonoBehaviour {
    public Vector2 size = new Vector2(0f, 0f);

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public float getWidth() {
        if( size.x == 0 )
            size.x = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.localScale.x;
        return size.x;
    }

    public float getHeight() {
        if( size.y == 0 )
            size.y = GetComponent<SpriteRenderer>().sprite.bounds.size.y * transform.lossyScale.y;
        return size.y;
    }

    public float rightExtent() {
        return transform.position.x + ( size.x / 2.0f );
    }

    public float leftExtent() {
        return transform.position.x - ( size.x / 2.0f );
    }
}
