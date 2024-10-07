using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public bool isAdditive = false;

    public void PerformSwitchScene(string sceneName)
    {
        LoadSceneMode mode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
        SceneManager.LoadScene(sceneName, mode);
    }

    public static void PerformStaticSwitchScene(string sceneName)
    {
        LoadSceneMode mode = LoadSceneMode.Single;
        SceneManager.LoadScene(sceneName, mode);
    }
}
