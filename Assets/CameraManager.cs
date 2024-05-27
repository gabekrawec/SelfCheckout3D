using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using System.Diagnostics;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;

    public CinemachineVirtualCamera topDownCam;
    public CinemachineVirtualCamera monitorCam;

    public CinemachineVirtualCamera startCam;
    private CinemachineVirtualCamera currentCam;
    // Start is called before the first frame update
    void Start()
    {
        SwitchCam(startCam);
    }

    // Update is called once per frame
    void Update()
    {
        //This is bad form, you really want to have a controller script that holds all your controls in one place. In that case, you'd just create a reference to CameraManager in there.
        if(Input.GetKeyDown(KeyCode.W))
        {
            SwitchCam(monitorCam);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            SwitchCam(topDownCam);
        }
    }

    public void SwitchCam(CinemachineVirtualCamera newCam)
    {
        currentCam = newCam;

        currentCam.Priority = 20;


        for(int i = 0; i < cameras.Length; i++)
        {
            if(cameras[i] != currentCam)
            {
                cameras[i].Priority = 10;
            }
        }
    }
}
