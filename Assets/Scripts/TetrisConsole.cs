using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TetrisConsole : MonoBehaviour
{
    [SerializeField] public int consoleId;
    [SerializeField] private Room room;
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] public CinemachineVirtualCamera cam;

    private bool active;
    private bool colliding;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Vector3 translation = new Vector3(moveDistance, 0, 0);
                room.transform.position += translation;
            }
            if (Input.GetKeyDown(KeyCode.A)) {
                Vector3 translation = new Vector3(-moveDistance, 0, 0);
                room.transform.position += translation;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Vector3 translation = new Vector3(0, 0, moveDistance);
                room.transform.position += translation;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Vector3 translation = new Vector3(0, 0, -moveDistance);
                room.transform.position += translation;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                room.transform.Rotate(0, -90, 0);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                room.transform.Rotate(0, 90, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log(room.checkCollision());
            }

        }

    }

    private bool checkCollision()
    {
        return true;
    }

    public void activate() { active = true; }
    public void deactivate() { active = false;  }
}
