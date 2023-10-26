using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using static UnityEditor.Progress;
using UnityEditor;
using TMPro;

public class TetrisConsole : MonoBehaviour
{
    [SerializeField] public int consoleId;
    [SerializeField] private Room room;
    [SerializeField] private Room[] allRooms;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] public CinemachineVirtualCamera cam;

    [Header("Messages")]
    [SerializeField] private float msgtime;
    [SerializeField] private TextMeshProUGUI txtmsg;

    private bool active;
    private int counter = 0;
    private bool exists;
    private bool colliding;
    private Vector3 startpos;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                txtmsg.enabled = false;
            }
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                counter++;
                room = allRooms[(counter) % allRooms.Length];
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log(checkCollision());
            }

        }

    }

    private bool checkCollision()
    {
        foreach(Room myRoom in allRooms)
        {
            if (myRoom.checkCollision()) { return true; }
        }
        return false;
    }

    public void activate() {
        active = true;
        startpos = room.transform.position;
    }
    public bool deactivate() {
        if (checkCollision()) {
            Debug.Log("You can't place the rooms like this!");
            txtmsg.enabled = true;
            timer = msgtime;
            return false;
        }
        active = false;
        return true;
    }
}
