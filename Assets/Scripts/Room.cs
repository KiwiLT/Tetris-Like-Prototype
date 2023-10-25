using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private BoxCollider boxcol;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool checkCollision()
    {
        RoomPiece[] pieces = GetComponentsInChildren<RoomPiece>();
        foreach(RoomPiece piece in pieces)
        {
            if (piece.checkOverlap()) { return true; }
        }
        return false;
    }


}
