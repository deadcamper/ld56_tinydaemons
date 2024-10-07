using UnityEngine;

public class Exit : MonoBehaviour
{
    // Start is called before the first frame update
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
          Application.Quit();
#endif
    }
}
