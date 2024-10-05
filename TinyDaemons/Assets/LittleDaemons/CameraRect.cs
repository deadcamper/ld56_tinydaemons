using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRect : MonoBehaviour
{
    public Camera cameraToRect;

    public RawImage imageRect;

    private void Start()
    {
        if (cameraToRect == null)
            cameraToRect = GetComponent<Camera>();

        cameraToRect.rect = imageRect.rectTransform.rect;
    }
}
