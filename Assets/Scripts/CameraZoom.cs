using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    protected Rigidbody2D p1;
    protected Rigidbody2D p2;
    protected CinemachineVirtualCamera vcam;
    protected CinemachineFramingTransposer transposer;
    protected float maxSize;
    protected float minSize;

    public float increment;
    public float timeLerp;

    protected bool shouldZoomIn = false;
    protected bool shouldZoomOut = false;

    protected float currentDiff;
    protected float lastFrameDiff = 0;

    void Start()
    {
        p1 = GameObject.Find("Player1").GetComponent<Rigidbody2D>();
        p2 = GameObject.Find("Player2").GetComponent<Rigidbody2D>();
        vcam = gameObject.GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        minSize = transposer.m_MinimumOrthoSize;
        maxSize = transposer.m_MaximumOrthoSize;
    }

    void Update()
    {
        currentDiff = (p1.position - p2.position).magnitude;
    }

    private void LateUpdate() {
        float difference = currentDiff - lastFrameDiff;
        zoom(difference * 0.1f) ;
        lastFrameDiff = currentDiff;
    }

    void zoom(float inc) {
        transposer.m_MinimumOrthoSize = Mathf.Clamp(transposer.m_MinimumOrthoSize + inc, minSize, maxSize);
    }

}
