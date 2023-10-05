using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRecentre : MonoBehaviour
{
    private CinemachineFreeLook cmr;

    // Start is called before the first frame update
    void Start()
    {
        cmr = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("CameraRecentre") == 1)
        {
            cmr.m_RecenterToTargetHeading.m_enabled = true;
        }
        else
        {
            cmr.m_RecenterToTargetHeading.m_enabled = false;
        }
    }
}
