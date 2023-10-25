using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] allcams;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToCam(CinemachineVirtualCamera targetCam)
    {
        foreach (CinemachineVirtualCamera camera in allcams)
        {
            camera.enabled = camera == targetCam;
        }
    }

    public void SwitchToPlayerCam()
    {
        CinemachineVirtualCamera targetCam = allcams[0];
        SwitchToCam(targetCam);
    }

}
