using UnityEngine;
using UnityEngine.UI;

public class CameraRectResizer : MonoBehaviour
{
    public Camera cameraToRect;

    public Image negativeOffset;

    public Rect lastWindowRect;

    private void Start()
    {
    }

    private void Update()
    {
        Rect newRect = new Rect(0,0,Screen.width,Screen.height);

        if (newRect != lastWindowRect)
        {
            Rect negativeSpace = negativeOffset.GetPixelAdjustedRect();

            Vector3[] fourCornersArray = new Vector3[4];
            negativeOffset.GetComponent<RectTransform>().GetWorldCorners(fourCornersArray);

            Vector2 ul = fourCornersArray[0];
            Vector2 size = fourCornersArray[2] - fourCornersArray[0];

            Rect negativeSpace2 = new Rect(ul, size);

            // HACK : only applies to right hand side
            Rect cameraSpace = new Rect(0, 0, negativeSpace2.x, Screen.height);

            cameraToRect.pixelRect = cameraSpace;

            lastWindowRect = newRect;
        }
    }
}
