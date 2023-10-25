using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPiece : MonoBehaviour
{
    private BoxCollider mycol;
    [SerializeField] private LayerMask layer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool checkOverlap()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1), Quaternion.identity, layer);
        //has to be 1 to ignore the roompiece the check starts in
        return hits.Length > 1;

    }
}
