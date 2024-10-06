using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CameraRectResizer : MonoBehaviour
{
    public Camera cameraToRect;

    [FormerlySerializedAs("negativeOffset")]
    public Image rightHandOffset;

    public Image bottomOffset;

    private Rect lastWindowRect;

    private void Start()
    {
    }

    private void Update()
    {
        Rect newRect = new Rect(0,0,Screen.width,Screen.height);

        if (newRect != lastWindowRect)
        {
            Rect negativeSpace = rightHandOffset.GetPixelAdjustedRect();

            Vector3[] fourCorners = new Vector3[4];
            rightHandOffset.GetComponent<RectTransform>().GetWorldCorners(fourCorners);

            Vector2 rightHand = fourCorners[0];

            bottomOffset.GetComponent<RectTransform>().GetWorldCorners(fourCorners);
            Vector2 bottomHand = fourCorners[1];

            // HACK : only applies to right hand side
            Rect cameraSpace = new Rect(0, bottomHand.y, rightHand.x, Screen.height - bottomHand.y);

            cameraToRect.pixelRect = cameraSpace;

            lastWindowRect = newRect;
        }
    }
}
